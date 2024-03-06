

/*----------------------------------------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 22.01.2022
 * Edit     : Chưa chỉnh Sửa
 * Content  : Service User : Thêm,Sửa,xóa, cập nhật User
 * --------------------------------------------------------------*/


using AutoMapper;
using APIProject.Service.Services;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Utils;
using APIProject.Service.Models;
using PagedList;
using PagedList.Core;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using APIProject.Service.Models.Authentication;
using System.Threading;
using Sentry;
using APIProject.Common.Models.Users;
using APIProject.Common.Utils;

namespace APIProject.Service.Services
{
    public class UserService : BaseService<Domain.Models.User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentry;
        public UserService(IUserRepository userRepository, IMapper mapper, IHub logger) : base(userRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _sentry = logger;
        }
        // Gender token :
        private string GenerateJwtToken(Domain.Models.User user, string secretKey, int timeout)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim("id",user.ID.ToString()),
                new Claim("type",SystemParam.TOKEN_TYPE_USER)
                }),
                Expires = DateTime.UtcNow.AddHours(timeout),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<JsonResultModel> Authenticate(LoginModel model, string secretKey, int timeout)
        {
            try
            {
                if (model.Phone == null || model.Password == null)
                    return JsonResponse.Error(SystemParam.ERROR_LOGIN_FIELDS_INVALID, SystemParam.MESSAGE_LOGIN_FIELDS_INVALID);
                var Us = await _userRepository.GetFirstOrDefaultAsync(x => x.Phone == model.Phone);
                if (Us == null) return JsonResponse.Error(SystemParam.ERROR_LOGIN_FAIL, SystemParam.MESSAGE_LOGIN_FAIL);
                if (!Util.CheckPass(model.Password, Us.Password)) return JsonResponse.Error(SystemParam.ERROR_LOGIN_FAIL_PASS, SystemParam.MESSAGE_LOGIN_FAIL_PASS);
                if (Us.Status.Equals(SystemParam.ACTIVE_FALSE)) return JsonResponse.Error(SystemParam.ERROR_LOGIN_ACCOUNT_LOCK, SystemParam.MESSAGE_LOGIN_ACCOUNT_LOCK);
                var token = GenerateJwtToken(Us, secretKey, timeout);
                Us.Token = token;
                await _userRepository.UpdateAsync(Us);
                var user = _mapper.Map<UserInfoModel>(Us);
                user.Password = String.Empty;
                return JsonResponse.Success(user);
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> CreateUser(CreateUserModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Name) || String.IsNullOrEmpty(model.Password))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_FIELDS_INVALID, SystemParam.MESSAGE_REGISTER_FIELDS_INVALID);
                if (!Util.validPhone(model.Phone))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_INVALID, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                var _phone = await _userRepository.GetFirstOrDefaultAsync(x => x.Phone == model.Phone);
                if (_phone != null) return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_EXIST, SystemParam.MESSAGE_REGISTER_PHONE_EXIST);
                if (!String.IsNullOrEmpty(model.Email))
                {
                    var _email = await _userRepository.GetFirstOrDefaultAsync(e => e.Email == model.Email);
                    if (_email != null) return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_EXIST, SystemParam.MESSAGE_REGISTER_EMAIL_EXIST);
                }
                // GenPassword 
                string password = Util.GenPass(model.Password);
                Domain.Models.User user = new Domain.Models.User()
                {
                    CreatedDate = DateTime.Now,
                    Email = model.Email,
                    Phone = model.Phone,
                    Role = model.Role,
                    Password = password,
                    Username = model.Name,
                    Status = SystemParam.ACTIVE
                };
                await _userRepository.AddAsync(user);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> DeleteUser(int ID)
        {
            try
            {
                var user = await _userRepository.GetFirstOrDefaultAsync(x => x.ID == ID && x.IsActive == SystemParam.ACTIVE);
                if (user == null) return JsonResponse.Error(SystemParam.ERROR_USER_NOT_FOUND, SystemParam.MESSAGE_USER_NOT_FOUND);
                // Xóa mềm
                user.IsActive = SystemParam.ACTIVE_FALSE;
                await _userRepository.UpdateAsync(user);
                return JsonResponse.Success();
            }
            catch (Exception Ex)
            {
                _sentry.CaptureException(Ex);
                return JsonResponse.ServerError();

            }
        }
        public async Task<JsonResultModel> GetUserDetail(int ID)
        {
            try
            {
                // Kiểm tra User có tồn tại hay khôngs
                var user = await _userRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (user == null) return JsonResponse.Error(SystemParam.ERROR_USER_NOT_FOUND, SystemParam.MESSAGE_USER_NOT_FOUND);

                // Nếu Trường hợp có tồn tại
                UserDetailModel _user = new UserDetailModel()
                {
                    ID = user.ID,
                    Name = user.Username,
                    Phone = user.Phone,
                    Email = user.Email,
                    Address = user.Address,
                    Role = user.Role,
                    Status = user.Status,
                };
                // Trả ra người dùng :
                return JsonResponse.Success(user);
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetUserInfo(int ID)
        {
            try
            {
                var user = await _userRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (user == null) return JsonResponse.Error(SystemParam.ERROR_USER_NOT_FOUND, SystemParam.MESSAGE_USER_NOT_FOUND);
                var model = _mapper.Map<UserInfoModel>(user);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetListUser(int page, int limit, string SearchKey, int? role, int? status, string fromDate, string toDate)
        {
            try
            {
                var list = await _userRepository.GetUsers(page, limit, SearchKey, role, status, fromDate, toDate);
                DataPagedListModel dataPagedListModel = new DataPagedListModel()
                {
                    Data = list,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = list.TotalItemCount
                };
                return JsonResponse.Success(dataPagedListModel);
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> UpdateUser(UpdateUserModel model)
        {
            try
            {
                // lấy bản ghi trong cơ sở dữ liệu
                var record = await _userRepository.GetFirstOrDefaultAsync(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.ID.Equals(model.ID));
                if (record == null) return JsonResponse.Error(SystemParam.ERROR_USER_NOT_FOUND, SystemParam.MESSAGE_USER_NOT_FOUND);
                if (!Util.validPhone(model.Phone))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_INVALID, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                if (!Util.ValidateEmail(model.Email))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_INVALID, SystemParam.MESSAGE_REGISTER_EMAIL_INVALID);
                var Email = await _userRepository.GetFirstOrDefaultAsync(x => x.Email.Equals(model.Email) && (x.ID != model.ID));
                var Phone = await _userRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(model.Phone) && (x.ID != model.ID));

                if (Email != null) return JsonResponse.Error(SystemParam.ERROR_CODE, SystemParam.MESSAGE_REGISTER_EMAIL_EXIST);
                if (Phone != null) return JsonResponse.Error(SystemParam.ERROR_CODE, SystemParam.MESSAGE_REGISTER_PHONE_EXIST);

                // Gán các giá trị cho bản ghi
                record.Username = model.Name;
                record.Phone = model.Phone;
                record.Email = model.Email;
                record.Role = model.RoleID;
                record.Status = model.Status;
                var res = await _userRepository.UpdateAsync(record);
                // cập nhật thành công 
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateDetailUser(int id, UpdateUserModelNew input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.Email) || String.IsNullOrEmpty(input.Address))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_FIELDS_INVALID, SystemParam.MESSAGE_REGISTER_FIELDS_INVALID);
                if (!Util.ValidateEmail(input.Email))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_EMAIL_INVALID, SystemParam.MESSAGE_REGISTER_EMAIL_INVALID);
                var model = await _userRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(id));
                _mapper.Map(input, model);
                await _userRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ChangePassword(int id, string oldPass, string newPass)
        {
            try
            {
                var user = await _userRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(id));
                if (!Util.CheckPass(oldPass, user.Password))
                {
                    return JsonResponse.Error(SystemParam.ERROR_CHANGE_PASSWORD_WRONG, SystemParam.MESSAGE_CHANGE_PASSWORD_WRONG);
                }
                user.Password = Util.GenPass(newPass);
                await _userRepository.UpdateAsync(user);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            try
            {
                var model = await _userRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) JsonResponse.Response(SystemParam.ERROR, SystemParam.ERROR_USER_NOT_FOUND, SystemParam.MESSAGE_USER_NOT_FOUND, "");
                if (model.Status == SystemParam.ACTIVE_FALSE)
                {
                    model.Status = SystemParam.ACTIVE;
                }
                else
                {
                    model.Status = SystemParam.ACTIVE_FALSE;
                }

                var res = await _userRepository.UpdateAsync(model);
                return JsonResponse.Success();

            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ResetPassword(int ID)
        {
            try
            {
                var model = await _userRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) JsonResponse.Response(SystemParam.ERROR, SystemParam.ERROR_USER_NOT_FOUND, SystemParam.MESSAGE_USER_NOT_FOUND, "");
                model.Password = Util.GenPass(model.Phone);
                await _userRepository.UpdateAsync(model);
                return JsonResponse.Success();

            }
            catch (Exception ex)
            {
                _sentry.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
