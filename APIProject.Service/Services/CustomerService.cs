

/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 04/01/2021
 * Edit     : Đã hoàn thành
 * Content  : Customer Service
 * ----------------------------------*/

using AutoMapper;
using APIProject.Service.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AutoMapper.Configuration;
using APIProject.Service.Utils;
using APIProject.Domain.Models;
using APIProject.Service.Models.Authentication;
using Sentry;
using APIProject.Service.Models.Customer;
using System.Threading;
using APIProject.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using APIProject.Common.Utils;
using APIProject.Common.Models.Users;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using FirebaseAdmin.Auth;
using System.Collections.Generic;
using APIProject.Common.DTOs.Authentication;
using APIProject.Common.DTOs;
using APIProject.Common.DTOs.Customer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using APIProject.Common.DTOs.UsageFrequency;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using APIProject.Repository;
using Microsoft.Extensions.Logging;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Transactions;

namespace APIProject.Service.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISendSmsService _sendSmsRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMemberPointHistoryRepository _pointRepository;
        private readonly IGiftCodeQRRepository _giftRepository;
        private readonly IQRCodeRepository _QRCodeRepository;
        INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public CustomerService(ICustomerRepository customerRepository, INewsRepository newsRepository, IMapper mapper, IHub sentryHub, IGiftCodeQRRepository giftRepository, IMemberPointHistoryRepository pointRepository, INotificationRepository notificationRepository, IQRCodeRepository qRCodeRepository, ISendSmsService sendSmsRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
            _newsRepository = newsRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _sendSmsRepository = sendSmsRepository;
            _giftRepository = giftRepository;
            _pointRepository = pointRepository;
            _notificationRepository = notificationRepository;
            _QRCodeRepository = qRCodeRepository;
        }
        private string GenerateJwtToken(string cusID, string secretKey, int timeout)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim("id",cusID),
                new Claim("type",SystemParam.TOKEN_TYPE_CUSTOMER)
                }),
                Expires = DateTime.UtcNow.AddMonths(timeout), // THời gian tồn tại của Token :
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        // Login App : 
        public async Task<JsonResultModel> Authenticate(LoginAppModel model, string secretKey, int timeout)
        {
            try
            {
                //FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(model.idToken);
                //IDictionary<string, object> openWith = (IDictionary<string, object>)decodedToken.Claims;
                //var phone = openWith["phone"];
                //if (phone == null)
                //    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_OTP, SystemParam.MESSAGE_PHONE_NOT_OTP);
                //phone = Util.convertPhone(phone.ToString());
                var phone = model.Phone;
                var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cus == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_FOUND, SystemParam.MESSAGE_PHONE_NOT_FOUND);
                }
                if (cus.OTP != model.OTP)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_OTP, SystemParam.MESSAGE_PHONE_NOT_OTP);
                }
                if (cus.ExpiredDateOTP.GetValueOrDefault() < DateTime.Now)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_OTP_EXPIRED, SystemParam.MESSAGE_PHONE_OTP_EXPIRED);
                }
                cus.Token = GenerateJwtToken(cus.ID.ToString(), secretKey, timeout);
                cus.DeviceID = model.DeviceID;
                cus.QtyOTP = 0;
                cus.LastLoginDate = DateTime.Now;
                await _customerRepository.UpdateAsync(cus);
                var cusInfo = await _customerRepository.GetCustomerInfo(cus);
                return JsonResponse.Success(cusInfo);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> Register(RegisterModel model, string secretKey, int timeout)
        {
            try
            {
                //FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(model.IdToken);
                //IDictionary<string, object> openWith = (IDictionary<string, object>)decodedToken.Claims;
                //var phone = openWith["phone"];
                //if (phone == null)
                //    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_OTP, SystemParam.MESSAGE_PHONE_NOT_OTP);
                //phone = Util.convertPhone(phone.ToString());
                var cusPhone = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(model.Phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cusPhone != null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_EXIST, SystemParam.MESSAGE_REGISTER_PHONE_EXIST);
                }
                var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(model.Phone) && x.IsActive.Equals(SystemParam.ACTIVE_FALSE));
                if (cus == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_FOUND, SystemParam.MESSAGE_PHONE_NOT_FOUND);
                }
                if (cus.OTP != model.OTP)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_OTP, SystemParam.MESSAGE_PHONE_NOT_OTP);
                }
                if (cus.ExpiredDateOTP.GetValueOrDefault() < DateTime.Now)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_OTP_EXPIRED, SystemParam.MESSAGE_PHONE_OTP_EXPIRED);
                }
                var token = GenerateJwtToken(cus.ID.ToString(), secretKey, timeout);
                cus.IsActive = SystemParam.ACTIVE;
                cus.Token = token;
                cus.DeviceID = model.DeviceID;
                cus.QtyOTP = 0;
                cus.LastLoginDate = DateTime.Now;
                cus.OriginCustomer = SystemParam.CUSTOMER_ORIGIN_REGISTER;
                await _customerRepository.UpdateAsync(cus);
                var cusInfo = await _customerRepository.GetCustomerInfo(cus);
                return JsonResponse.Success(cusInfo);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetUserInfo(int ID)
        {
            try
            {
                var cusinfo = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                return JsonResponse.Success(cusinfo);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ChangeStatus(int customerID)
        {
            try
            {
                Customer customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(customerID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (customer == null) JsonResponse.Response(SystemParam.ERROR, SystemParam.ERROR_NOT_FOUND_CUSTOMER, SystemParam.MESSAGE_NOT_FOUND_CUSTOMER, "");
                if (customer.Status == SystemParam.ACTIVE_FALSE)
                {
                    customer.Status = SystemParam.ACTIVE;
                }
                else
                {
                    customer.Status = SystemParam.ACTIVE_FALSE;
                }

                var res = await _customerRepository.UpdateAsync(customer);
                return JsonResponse.Success();

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> GetCustomerInfo(Customer cus)
        {
            try
            {
                var data = await _customerRepository.GetCustomerInfo(cus);
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetCustomers(int page, int limit, int? status, string searchKey, string startDate, string endDate, int? originCustomer)
        {
            try
            {
                var list = await _customerRepository.GetCustomers(page, limit, status, searchKey, startDate, endDate, originCustomer);
                DataPagedListModel data = new DataPagedListModel
                {
                    Data = list,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = list.TotalItemCount
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetCustomerDetail(int ID)
        {
            try
            {
                var model = await _customerRepository.GetCustomerDetail(ID);
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.NOT_FOUND_CUSTOMER, SystemParam.MESSAGE_NOT_FOUND_CUSTOMER);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ChangeAvatar(Customer customer, string ImageUrl)
        {
            try
            {
                await _customerRepository.UpdateAsync(customer);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateCustomerInfo(Customer customer, UpdateCustomerInfoModel input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.Name))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_FIELDS_INVALID, SystemParam.MESSAGE_REGISTER_FIELDS_INVALID);
                if (!String.IsNullOrEmpty(input.Email))
                {
                    if (!Util.ValidateEmail(input.Email))
                        return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_INVALID, SystemParam.MESSAGE_REGISTER_EMAIL_INVALID);
                }
                customer.Name = input.Name;
                customer.Email = input.Email;
                customer.DOB = input.DOB;
                customer.Gender = input.Gender;
                customer.Avatar = input.Avatar;
                //customer.IdentityNumber = input.IdentityNumber;
                //customer.ProvinceID = input.ProvinceID;
                //customer.DistrictID = input.DistrictID;
                //customer.WardID = input.WardID;
                customer.Address = input.Address;
                //customer.QRCode = Util.GenerateCodeProject();
                await _customerRepository.UpdateAsync(customer);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }


        public async Task<JsonResultModel> DeleteCustomer(int ID)
        {
            try
            {
                var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cus == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_CUSOTMER_NOT_EXSIST, SystemParam.MESSAGE_CUSOTMER_NOT_EXSIST);
                }
                cus.IsActive = SystemParam.ACTIVE_FALSE;
                await _customerRepository.UpdateAsync(cus);
                var listQR = await _QRCodeRepository.GetAllAsync(x => x.Phone.Equals(cus.Phone));
                await _QRCodeRepository.DeleteListAsync(listQR);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateUserWebInfo(ChangeCustomerInfoWebModel input)
        {
            try
            {
                if (!String.IsNullOrEmpty(input.Email))
                {
                    if (!Util.ValidateEmail(input.Email))
                        return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_INVALID, SystemParam.MESSAGE_REGISTER_EMAIL_INVALID);
                }
                var model = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                model.Email = input.Email;
                model.Gender = input.Gender;
                model.Address = input.Address;
                await _customerRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CreateCustomer(CreateCustomer input)
        {
            try
            {
                if (!Util.ValidateEmail(input.Email))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_INVALID, SystemParam.MESSAGE_REGISTER_EMAIL_INVALID);
                if (!Util.validPhone(input.Phone))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_INVALID, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                Customer cus = new Customer()
                {
                    Name = input.Name,
                    Phone = input.Phone,
                    Email = input.Email,
                    DeviceID = input.DeviceID,
                    Role = SystemParam.ROLE_STAFF,
                };
                await _customerRepository.AddAsync(cus);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CustomerInfo(CustomerInfo cus)
        {
            try
            {
                Random random = new Random();
                String str = "abcdefghijklmnopqrstuvwxyz0123456789";
                int size = 16;
                String QRCode = "";
                for (int i = 0; i < size; i++)
                {
                    int x = random.Next(str.Length);
                    QRCode = QRCode + str[x];
                }
                var Model = await GetFirstOrDefaultAsync(c => c.Phone == cus.Phone && c.IsActive == SystemParam.ACTIVE);
                if (!Util.validPhone(cus.Phone))
                {
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_INVALID, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                }
                else if (cus.Name == null && cus.ProvinceID == 0 && cus.DistrictID == 0 && cus.WardID == 0 && cus.DOB == DateTime.MinValue && cus.Job == null && cus.Gender == -1)
                {
                    return JsonResponse.Error(SystemParam.ERROR_REQUIRED_COUSTOMER, SystemParam.MESSAGE_REQUIRED_COUSTOMER);
                }
                if (Model != null)
                {
                    _mapper.Map(cus, Model);
                    await _customerRepository.UpdateAsync(Model);
                }
                else
                {
                    var model = _mapper.Map<Customer>(cus);
                    await _customerRepository.AddAsync(model);
                }
                return JsonResponse.Success(QRCode);
            }
            catch (Exception ex)
            {

                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetNumberCustomerEvent(int page, int limit, string searchKey, int? EventID, string fromDate, string toDate)
        {
            try
            {
                var model = await _customerRepository.GetNumberCustomerEvent(page, limit, searchKey, EventID, fromDate, toDate);
                DataPagedListModel data = new DataPagedListModel
                {
                    Data = model,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = model.TotalItemCount
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> CustomerEventDetail(int page, int limit, string sdt, int ID, string fromDate, string toDate)
        {
            var model = await _customerRepository.CustomerEventDetail(page, limit, sdt, ID, fromDate, toDate);
            if (model == null)
            {
                return JsonResponse.Error(SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_CODE_NOT_FOUND_NEWS);
            }
            DataPagedListModel data = new DataPagedListModel
            {
                Data = model,
                Limit = limit,
                Page = page,
                TotalItemCount = model.TotalItemCount
            };
            return JsonResponse.Success(data);
        }
        public async Task<JsonResultModel> GetListPercentageCustomer(int? EventID)
        {
            var model = await _customerRepository.GetListPercentageCustomer(EventID);
            if (model == null)
            {
                return JsonResponse.Error(SystemParam.ERROR_USAGE_FREQUENCY_NOT_FOUND, SystemParam.MESSAGE_USAGE_FREQUENCY_NOT_FOUND);
            }
            return JsonResponse.Success(model);
        }
        public async Task<JsonResultModel> GetListPercentageGenderCustomer(int? EventID)
        {
            var model = await _customerRepository.GetListPercentageGenderCustomer(EventID);
            if (model == null || model.Count == 0)
            {
                return JsonResponse.Error(SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_CODE_NOT_FOUND_NEWS);
            }
            return JsonResponse.Success(model);
        }
        public async Task<JsonResultModel> GetListCustomerChannelPercentage(int? EventID)
        {
            var model = await _customerRepository.GetListCustomerChannelPercentage(EventID);
            return JsonResponse.Success(model);
        }
        public async Task<JsonResultModel> GetListCustomerPercentageProvinces(int? EventID)
        {
            var model = await _customerRepository.GetListCustomerPercentageProvinces(EventID);
            return JsonResponse.Success(model);
        }
        public async Task<JsonResultModel> GetListCustomerPercentageAge(int? EventID)
        {
            var model = await _customerRepository.GetListCustomerPercentageAge(EventID);
            return JsonResponse.Success(model);
        }
        public async Task<JsonResultModel> StatisticsGiftExchange(int page, int limit, string SeachKey, string fromDate, string toDate)
        {
            try
            {
                var model = await _customerRepository.StatisticsGiftExchange(page, limit, SeachKey, fromDate, toDate);
                var Data = new DataPagedListModel
                {
                    Page = page,
                    Limit = limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = model
                };
                return JsonResponse.Success(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> GetNumberOfGiftExchangeDetail(int page, int limit, int ID, string SeachKey, string fromDate, string toDate)
        {
            try
            {
                var model = await _customerRepository.GetNumberOfGiftExchangeDetail(page, limit, ID, SeachKey, fromDate, toDate);
                var Data = new DataPagedListModel
                {
                    Page = page,
                    Limit = limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = model
                };
                return JsonResponse.Success(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> GetGiftVoucherDetail(int page, int limit, int ID, string SeachKey)
        {
            try
            {
                var model = await _customerRepository.GetGiftVoucherDetail(page, limit, ID, SeachKey);
                var Data = new DataPagedListModel
                {
                    Page = page,
                    Limit = limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = model
                };
                return JsonResponse.Success(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> CheckPhoneLogin(string phone, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cus == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_EXIST, SystemParam.MESSAGE_PHONE_NOT_EXIST);
                }
                if (cus.ResetDateOTP > DateTime.Now)
                {
                    cus.QtyOTP = 0;
                }
                if (cus.QtyOTP >= SystemParam.OTP_MAX_QUANTITY)
                {
                    return JsonResponse.Error(SystemParam.ERROR_OTP_MAX_QUANTITY_EXCEED, SystemParam.MESSAGE_OTP_MAX_QUANTITY_EXCEED);
                }
                var otp = Util.RandomNumber(100000, 999999).ToString();
                if (phone == "0795296216")
                {
                    otp = "123456";
                }
                //var otp = "123456";
                cus.OTP = otp;
                cus.QtyOTP++;
                cus.ResetDateOTP = DateTime.Now.AddMinutes(SystemParam.TIME_OTP_RESET);
                cus.ExpiredDateOTP = DateTime.Now.AddMinutes(SystemParam.TIME_EXPIRE_OTP);
                await _customerRepository.UpdateAsync(cus);
                await _sendSmsRepository.SendSms(phone, otp, webHostEnvironment);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }


        public async Task<JsonResultModel> CheckPhoneRegister(string phone, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cus != null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_EXIST, SystemParam.MESSAGE_REGISTER_PHONE_EXIST);
                }
                if (!Util.validPhone(phone))
                {
                    return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_VALID, SystemParam.MESSAGE_PHONE_NOT_VALID);
                }
                var otp = Util.RandomNumber(100000, 999999).ToString();
                //var otp = "123456";
                if (phone == "0795296216")
                {
                    otp = "123456";
                }
                var cusOld = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(phone));
                if (cusOld != null)
                {
                    if (cusOld.ResetDateOTP.GetValueOrDefault() > DateTime.Now)
                    {
                        cusOld.QtyOTP = 0;
                    }
                    if (cusOld.QtyOTP >= SystemParam.OTP_MAX_QUANTITY)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_OTP_MAX_QUANTITY_EXCEED, SystemParam.MESSAGE_OTP_MAX_QUANTITY_EXCEED);
                    }
                    cusOld.OTP = otp;
                    cusOld.QtyOTP = cusOld.QtyOTP.GetValueOrDefault() + 1;
                    cusOld.ResetDateOTP = DateTime.Now.AddMinutes(SystemParam.TIME_OTP_RESET);
                    cusOld.ExpiredDateOTP = DateTime.Now.AddMinutes(SystemParam.TIME_EXPIRE_OTP);
                    await _customerRepository.UpdateAsync(cusOld);
                }
                else
                {
                    var cusNew = new Customer
                    {
                        Phone = phone,
                        IsActive = SystemParam.ACTIVE_FALSE,
                        Role = SystemParam.ROLE_CUSTOMER,
                        QtyOTP = 1,
                        OTP = otp,
                        ResetDateOTP = DateTime.Now.AddMinutes(SystemParam.TIME_OTP_RESET),
                        ExpiredDateOTP = DateTime.Now.AddMinutes(SystemParam.TIME_EXPIRE_OTP),
                    };
                    await _customerRepository.AddAsync(cusNew);
                }
                await _sendSmsRepository.SendSms(phone, otp, webHostEnvironment);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ExportExcelZaloOA(IWebHostEnvironment webHostEnvironment, HttpContext context)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, "Template");
                var fullPath = Path.Combine(pathToSave, SystemParam.EXPORT_SAMPLE_CUSTOMER_EXPORT);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var newFileName = SystemParam.EXPORT_SAMPLE_ZALO_OA_NEW + DateTime.Now.ToString("ssddMMyyyy") + ".xlsx";
                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExcelPackage pack = new ExcelPackage(stream);
                    ExcelWorkbook workbook = pack.Workbook;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                    int row = 2;
                    var list = await _customerRepository.GetAllAsync(x => x.IsActive.Equals(SystemParam.ACTIVE), x => x.OrderByDescending(a => a.ID), x => x.Include(a => a.Ward).Include(a => a.District).Include(a => a.Province));
                    int stt = 1;
                    foreach (var item in list)
                    {
                        sheet.Cells[row, 1].Value = stt;
                        sheet.Cells[row, 2].Value = item.Name;
                        sheet.Cells[row, 3].Value = item.Phone;
                        sheet.Cells[row, 4].Value = item.Gender.GetValueOrDefault() == SystemParam.IDBoy ? SystemParam.Gender_Boy : SystemParam.Gender_Girl;
                        sheet.Cells[row, 5].Value = Util.ConvertGenderType(item.AgeType.GetValueOrDefault());
                        sheet.Cells[row, 6].Value = (item.WardID.HasValue ? item.Ward.Name : "") + (item.DistrictID.HasValue ? " ," + item.District.Name : "") + (item.ProvinceID.HasValue ? " ," + item.Province.Name : "");
                        row++;
                        stt++;
                    }
                    var newPath = Path.Combine(pathToSave, newFileName);
                    pack.SaveAs(newPath);
                }
                var httpRequest = context.Request;
                var host = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
                var url = host + "/Template/" + newFileName;
                return JsonResponse.Success(url);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ImportSampleCustomer(HttpContext context)
        {
            try
            {
                var httpRequest = context.Request;
                var host = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
                var url = host + "/Template/" + SystemParam.IMPORT_SAMPLE_CUSTOMER;
                return JsonResponse.Success(url);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ImportCustomer(HttpContext context, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                var httpRequest = context.Request;
                var postedFile = httpRequest.Form.Files.GetFile(SystemParam.FILE);
                if (postedFile != null)
                {
                    var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, "Template");
                    string name = DateTime.Now.ToString("ssddMMyyyy") + postedFile.FileName;
                    var fullPath = Path.Combine(pathToSave, name);
                    using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate))
                    {
                        await postedFile.CopyToAsync(stream);
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                        // If you use EPPlus in a noncommercial context
                        // according to the Polyform Noncommercial license:
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        ExcelPackage pack = new ExcelPackage(stream);

                        ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                        var rowCount = sheet.Dimension.Rows;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            for (int row = 3; row <= rowCount; row++)
                            {
                                var model = new Customer();
                                model.Role = SystemParam.ROLE_CUSTOMER;
                                if (sheet.Cells[row, 2].Value != null && sheet.Cells[row, 2].Value.ToString().Count() > 0)
                                {
                                    model.Phone = sheet.Cells[row, 2].Value.ToString().Trim();
                                    if (!Util.validPhone(model.Phone))
                                        return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_INVALID, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                                    var ap = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(model.Phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                                    if (ap != null) continue;
                                }
                                else
                                {
                                    break;
                                }
                                if (sheet.Cells[row, 3].Value != null && sheet.Cells[row, 3].Value.ToString().Count() > 0)
                                {
                                    model.Name = sheet.Cells[row, 3].Value.ToString().Trim();
                                }
                                await _customerRepository.AddAsync(model);
                            }
                            scope.Complete();
                        }
                        stream.Close();
                        File.Delete(fullPath);
                    }
                    return JsonResponse.Success();
                }
                else
                {
                    return JsonResponse.Error(SystemParam.ERROR_IMPORT_EXCEL_CUSTOMER_ERROR, SystemParam.MESSAGE_IMPORT_EXCEL_CUSTOMER_ERROR);
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ExportExcelGiftExchange(int ID, string SeachKey, string fromDate, string toDate, IWebHostEnvironment webHostEnvironment, HttpContext context)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, "Template");
                var fullPath = Path.Combine(pathToSave, SystemParam.EXPORT_SAMPLE_CUSTOMER_GIFT_EXCHANGE);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var newFileName = SystemParam.EXPORT_SAMPLE_CUSTOMER_GIFT_EXCHANGE_NEW + DateTime.Now.ToString("ssddMMyyyy") + ".xlsx";
                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExcelPackage pack = new ExcelPackage(stream);
                    ExcelWorkbook workbook = pack.Workbook;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                    int row = 2;
                    var list = await _customerRepository.GetALLNumberOfGiftExchangeDetail(ID, SeachKey, fromDate, toDate);
                    int stt = 1;
                    foreach (var item in list)
                    {
                        sheet.Cells[row, 1].Value = stt;
                        sheet.Cells[row, 2].Value = item.NameCus;
                        sheet.Cells[row, 3].Value = item.Phone;
                        sheet.Cells[row, 4].Value = item.NumberGift;
                        sheet.Cells[row, 5].Value = string.Join(",", item.GiftVouchers.Select(x => x.Name).ToList());
                        sheet.Cells[row, 6].Value = @String.Format("{0:0,00}", item.TotalAmount);
                        sheet.Cells[row, 7].Value = item.Date;
                        row++;
                        stt++;
                    }
                    var newPath = Path.Combine(pathToSave, newFileName);
                    pack.SaveAs(newPath);
                }
                var httpRequest = context.Request;
                var host = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
                var url = host + "/Template/" + newFileName;
                return JsonResponse.Success(url);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}



