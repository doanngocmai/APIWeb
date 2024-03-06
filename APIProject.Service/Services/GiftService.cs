using APIProject.Common.DTOs.Gift;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using PagedList.Core;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace APIProject.Service.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IGiftCodeQRRepository _giftCodeQRRepository;
        private readonly IMemberPointHistoryRepository _memberPointHistoryRepository;
        private readonly IGiftEventRepository _giftEventRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public GiftService(IGiftRepository giftRepository, IMapper mapper, IHub sentryHub, IGiftCodeQRRepository giftCodeQRRepository, ICustomerRepository customerRepository, IMemberPointHistoryRepository memberPointHistoryRepository, IGiftEventRepository giftEventRepository)
        {
            _giftRepository = giftRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _giftCodeQRRepository = giftCodeQRRepository;
            _customerRepository = customerRepository;
            _memberPointHistoryRepository = memberPointHistoryRepository;
            _giftEventRepository = giftEventRepository;
        }

        public async Task<JsonResultModel> GetListGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate)
        {
            try
            {
                var listGift = await _giftRepository.GetListGift(page, limit, searchKey, type, fromDate, toDate);
                var data = new DataPagedListModel
                {
                    Data = listGift,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = listGift.TotalItemCount,
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetGiftDetail(int ID)
        {
            try
            {
                var model = await _giftRepository.GetGiftDetail(ID);
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_GIFT_NOT_FOUND);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListExchangeGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate)
        {
            try
            {
                var model = await _giftRepository.GetListExchangeGift(page, limit, searchKey, type, fromDate, toDate);
                var data = new DataPagedListModel
                {
                    Data = model,
                    Page = page,
                    Limit = limit,
                    TotalItemCount = model.TotalItemCount,
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListGiftCode(int page, int limit, string searchKey, int? status, int GiftID)
        {
            try
            {
                var model = await _giftRepository.GetListGiftCode(page, limit, searchKey, status, GiftID);
                var data = new DataPagedListModel
                {
                    Data = model,
                    Page = page,
                    Limit = limit,
                    TotalItemCount = model.TotalItemCount,
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CreateGift(CreateGiftModel input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.Name) || input.Type == 0 || input.UrlImage == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_FIELDS_INVALID, SystemParam.MESSAGE_GIFT_FIELDS_INVALID);
                }
                if (input.Type.Equals(SystemParam.TYPE_GIFT_PRESENTS) && input.Number == 0)
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_FIELDS_INVALID, SystemParam.MESSAGE_GIFT_FIELDS_INVALID);
                }
                if (input.Type.Equals(SystemParam.TYPE_GIFT_VOURCHER) && (input.FromDate == DateTime.MinValue || input.ToDate == DateTime.MinValue))
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_FIELDS_INVALID, SystemParam.MESSAGE_GIFT_FIELDS_INVALID);
                }
                input.FromDate = Util.ConvertFromDate(input.FromDate.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                input.ToDate = Util.ConvertToDate(input.ToDate.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                var model = _mapper.Map<Gift>(input);
                model.Status = SystemParam.ACTIVE;
                await _giftRepository.AddAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {

                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateGift(UpdateGiftModel input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.Name) || input.Type == 0 || input.UrlImage == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_FIELDS_INVALID, SystemParam.MESSAGE_GIFT_FIELDS_INVALID);
                }
                if (input.Type.Equals(SystemParam.TYPE_GIFT_PRESENTS) && input.Number == 0)
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_FIELDS_INVALID, SystemParam.MESSAGE_GIFT_FIELDS_INVALID);
                }
                if (input.Type.Equals(SystemParam.TYPE_GIFT_VOURCHER) && (input.FromDate == DateTime.MinValue || input.ToDate == DateTime.MinValue))
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_FIELDS_INVALID, SystemParam.MESSAGE_GIFT_FIELDS_INVALID);
                }
                var model = await _giftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_GIFT_NOT_FOUND);
                input.FromDate = Util.ConvertFromDate(input.FromDate.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                input.ToDate = Util.ConvertToDate(input.ToDate.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                _mapper.Map(input, model);
                await _giftRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> DeleteGift(int ID)
        {
            try
            {
                var model = await _giftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_GIFT_NOT_FOUND);
                var checkEvent = await _giftEventRepository.GetFirstOrDefaultAsync(x => x.GiftID.Equals(ID));
                if (checkEvent != null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_EXIST_EVENT, SystemParam.MESSAGE_GIFT_EXIST_EVENT);
                }
                model.IsActive = SystemParam.ACTIVE_FALSE;
                await _giftRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            try
            {
                var model = await _giftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_NEWS_NOT_FOUND, SystemParam.MESSAGE_NEWS_NOT_FOUND);
                }
                if (model.Status == SystemParam.ACTIVE)
                    model.Status = SystemParam.ACTIVE_FALSE;
                else
                    model.Status = SystemParam.ACTIVE;
                await _giftRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CreateGiftCode(CreateGiftCodeModel input)
        {
            try
            {
                var code = input.Code.Trim();
                if (String.IsNullOrEmpty(code))
                {
                    return JsonResponse.Error(SystemParam.ERROR_GIFT_FIELDS_INVALID, SystemParam.MESSAGE_GIFT_FIELDS_INVALID);
                }
                var giftCode = await _giftCodeQRRepository.GetFirstOrDefaultAsync(x => x.Code.Equals(input.Code) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (giftCode != null) { return JsonResponse.Error(SystemParam.ERROR_GIFT_CODE_DUPLICATE, SystemParam.MESSAGE_GIFT_CODE_DUPLICATE); }
                var model = new GiftCodeQR()
                {
                    Code = code,
                    Status = SystemParam.STATUS_GIFTCODE_NOT_EXCHANGE,
                    GiftID = input.GiftID,
                };
                await _giftCodeQRRepository.AddAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateGiftCode(UpdateGiftCodeModel input)
        {
            try
            {
                var model = await _giftCodeQRRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) { return JsonResponse.Error(SystemParam.ERROR_GIFT_NOT_FOUND, SystemParam.MESSAGE_GIFT_NOT_FOUND); }
                var giftCode = await _giftCodeQRRepository.GetFirstOrDefaultAsync(x => x.Code.Equals(input.Code) && x.ID != input.ID && x.IsActive.Equals(SystemParam.ACTIVE));
                if (giftCode != null) { return JsonResponse.Error(SystemParam.ERROR_GIFT_CODE_DUPLICATE, SystemParam.MESSAGE_GIFT_CODE_NOT_FOUND); }
                model.Code = input.Code;
                await _giftCodeQRRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteGiftCode(int ID)
        {
            try
            {
                var model = await _giftCodeQRRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) { return JsonResponse.Error(SystemParam.ERROR_GIFT_NOT_FOUND, SystemParam.MESSAGE_GIFT_NOT_FOUND); }
                model.IsActive = SystemParam.ACTIVE_FALSE;
                await _giftCodeQRRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListVoucher(int page, int limit)
        {
            try
            {
                var model = await _giftRepository.GetListVoucher(page, limit);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {

                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> GetVoucherDetail(int ID)
        {
            try
            {
                var model = await _giftRepository.GetVoucherDetail(ID);
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_VOUCHER_NOT_FOUND);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetMyListVoucher(int page, int limit, int status, int CustomerID)
        {
            try
            {
                var model = await _giftRepository.GetMyListVoucher(page, limit, status, CustomerID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {

                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetMyListVoucherExpired(int page, int limit, int CustomerID)
        {
            try
            {
                var model = await _giftRepository.GetMyListVoucherExpired(page, limit, CustomerID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {

                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetMyVoucherDetail(int ID)
        {
            try
            {
                var model = await _giftRepository.GetMyVoucherDetail(ID);
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_VOUCHER_NOT_FOUND);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ExchangeVoucher(int ID, int CustomerID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = await _giftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE) && x.Status.Equals(SystemParam.ACTIVE));
                    if (model == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_VOUCHER_NOT_FOUND);
                    }
                    if (model.Point == 0)
                    {
                        var giftCodeOld = await _giftCodeQRRepository.GetFirstOrDefaultAsync(x => x.CustomerID.Equals(CustomerID) && x.GiftID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                        if (giftCodeOld != null)
                        {
                            return JsonResponse.Error(SystemParam.ERROR_ALREADY_EXCHANGED_VOUCHER_FREE, SystemParam.MESSAGE_ALREADY_EXCHANGED_VOUCHER_FREE);
                        }
                    }
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(CustomerID));
                    if (cus.Point < model.Point)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_EXCHANGED_VOUCHER_NOT_ENOUGH_POINT, SystemParam.MESSAGE_EXCHANGED_VOUCHER_NOT_ENOUGH_POINT);
                    }
                    cus.Point -= model.Point;
                    await _customerRepository.UpdateAsync(cus);
                    var memberPointHistory = new MemberPointHistory
                    {
                        Title = model.Name,
                        Point = model.Point,
                        Balance = cus.Point,
                        CustomerID = CustomerID,
                        GiftID = model.ID,
                        Type = SystemParam.TYPE_MEMBER_HISTORY_EXCHANGED_POINT
                    };
                    await _memberPointHistoryRepository.AddAsync(memberPointHistory);
                    var giftCodeQR = await _giftCodeQRRepository.GetFirstOrDefaultAsync(x => x.GiftID.Equals(ID) && x.Status.Equals(SystemParam.STATUS_GIFTCODE_NOT_EXCHANGE) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (giftCodeQR == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_EXCHANGED_VOUCHER_EMPTY, SystemParam.MESSAGE_EXCHANGED_VOUCHER_EMPTY);
                    }
                    giftCodeQR.Status = SystemParam.STATUS_GIFTCODE_EXCHANGE;
                    giftCodeQR.CustomerID = cus.ID;
                    giftCodeQR.CreatedDate = DateTime.Now;
                    await _giftCodeQRRepository.UpdateAsync(giftCodeQR);
                    scope.Complete();
                    return JsonResponse.Success(model);
                }


            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UseVoucher(int ID)
        {
            try
            {
                var giftCode = await _giftCodeQRRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                if (giftCode == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_VOUCHER_NOT_FOUND);
                }
                //if (giftCode.Status == SystemParam.STATUS_GIFTCODE_USED)
                //{
                //    return JsonResponse.Error(SystemParam.ERROR_USED_VOUCHER, SystemParam.MESSAGE_USED_VOUCHER);
                //}
                giftCode.Status = SystemParam.STATUS_GIFTCODE_USED;
                await _giftCodeQRRepository.UpdateAsync(giftCode);
                return JsonResponse.Success(giftCode.Code);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListGift()
        {
            try
            {
                var model = await _giftRepository.GetListGift();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ImportVoucherCode(HttpContext context)
        {
            try
            {

                var httpRequest = context.Request;
                var host = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
                var url = host + "/Template/" + SystemParam.IMPORT_SAMPLE_APARTMENT;
                return JsonResponse.Success(url);

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ImportVoucherCode(int GiftID, HttpContext context, IWebHostEnvironment webHostEnvironment)
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
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        ExcelPackage pack = new ExcelPackage(stream);
                        ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                        var rowCount = sheet.Dimension.Rows;

                        for (int row = 3; row <= rowCount; row++)
                        {

                            if (sheet.Cells[row, 2].Value != null && sheet.Cells[row, 2].Value.ToString().Count() > 0)
                            {
                                var code = sheet.Cells[row, 2].Value.ToString().Trim();
                                var checkCode = await _giftCodeQRRepository.GetFirstOrDefaultAsync(c => c.Code.Equals(code) && c.IsActive.Equals(SystemParam.ACTIVE));
                                if (checkCode != null)
                                {
                                    break;
                                }
                                var model = new GiftCodeQR();
                                model.Code = code;
                                model.Status = SystemParam.ACTIVE_FALSE;
                                model.GiftID = GiftID;
                                await _giftCodeQRRepository.AddAsync(model);
                            }

                        }
                        stream.Close();
                        File.Delete(fullPath);
                    }
                    return JsonResponse.Success();
                }
                else
                {
                    return JsonResponse.Error(SystemParam.ERROR_IMPORT_EXCEL_VOUCHER_ERROR, SystemParam.MESSAGE_IMPORT_EXCEL_VOUCHER_ERROR);
                }

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
