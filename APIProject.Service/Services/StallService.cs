using APIProject.Common.DTOs.Stall;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class StallService : IStallService
    {
        private readonly IStallRepository _stallRepository;
        private readonly IRelatedStallRepository _relatedStallRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public StallService(IMapper mapper, IHub sentryHub, IStallRepository stallRepository, IRelatedStallRepository relatedStallRepository)
        {
            _stallRepository = stallRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _relatedStallRepository = relatedStallRepository;
        }

        public async Task<JsonResultModel> GetListStall(int page, int limit, int? floor, int? status, string searchKey, int? type)
        {
            try
            {
                var model = await _stallRepository.GetListStall(page, limit, floor, status, searchKey, type);
                var data = new DataPagedListModel
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

        public async Task<JsonResultModel> GetListStall(int? page, int? limit, int? floor, string searchKey, int? categoryID, int? orderBy)
        {
            try
            {
                if (page.HasValue && limit.HasValue)
                {
                    var model = await _stallRepository.GetListStall(page.Value, limit.Value, floor, searchKey, categoryID, orderBy);
                    return JsonResponse.Success(model);
                }
                else
                {
                    var model = await _stallRepository.GetListStall();
                    return JsonResponse.Success(model);
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetStallRelated(int ID)
        {
            try
            {
                var model = await _stallRepository.GetStallRelated(ID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetStallDetail(int ID)
        {
            try
            {
                var stall = await _stallRepository.GetStallDetail(ID);
                if (stall == null)
                    return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                return JsonResponse.Success(stall);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetSumBillStall(int type, int? EventID)
        {
            try
            {
                var model = await _stallRepository.GetSumBillStall(type, EventID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ExportExcelSumBillStall(IWebHostEnvironment webHostEnvironment, HttpContext context)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, "Template");
                var fullPath = Path.Combine(pathToSave, SystemParam.EXPORT_SAMPLE_SUM_BILL_STALL);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var newFileName = SystemParam.EXPORT_SAMPLE_SUM_BILL_STALL_NEW + DateTime.Now.ToString("ssddMMyyyy") + ".xlsx";
                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExcelPackage pack = new ExcelPackage(stream);
                    ExcelWorkbook workbook = pack.Workbook;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                    int row = 2;
                    var list = await _stallRepository.GetAllSumBillStall();
                    int stt = 1;
                    foreach (var item in list)
                    {
                        sheet.Cells[row, 1].Value = stt;
                        sheet.Cells[row, 2].Value = item.Name;
                        sheet.Cells[row, 3].Value = @String.Format("{0:0,0}", item.SumBill);
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
        public async Task<JsonResultModel> CreateStall(CreateStallModel input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Name.Trim()) || input.CategoryID == 0 || input.Floor == 0 || string.IsNullOrEmpty(input.Logo.Trim()))
                {
                    return JsonResponse.Error(SystemParam.ERROR_STALL_FIELDS_INVALID, SystemParam.MESSAGE_STALL_FIELDS_INVALID);
                }
                var model = _mapper.Map<Stall>(input);
                model.Name = input.Name.Trim();
                model.Logo = input.Logo.Trim();
                model.Code = Util.GenerateCode("GH");
                model.Status = SystemParam.ACTIVE;
                await _stallRepository.AddAsync(model);
                if (input.ListPromotionID != null)
                {
                    foreach (var item in input.ListPromotionID)
                    {
                        var relatedStall = new RelatedStall
                        {
                            NewsID = item,
                            StallID = model.ID
                        };
                        await _relatedStallRepository.AddAsync(relatedStall);
                    }
                }
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
                Stall model = await _stallRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                if (model.Status == SystemParam.ACTIVE)
                    model.Status = SystemParam.ACTIVE_FALSE;
                else
                    model.Status = SystemParam.ACTIVE;
                var res = await _stallRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> UpdateStall(UpdateStallModel input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Name.Trim()) || input.CategoryID == 0 || input.Floor == 0 || string.IsNullOrEmpty(input.Logo.Trim()))
                    return JsonResponse.Error(SystemParam.ERROR_STALL_FIELDS_INVALID, SystemParam.MESSAGE_STALL_FIELDS_INVALID);
                var model = await _stallRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                    return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                _mapper.Map(input, model);
                model.Name = input.Name.Trim();
                model.Logo = input.Logo.Trim();
                await _stallRepository.UpdateAsync(model);
                var listStallRelated = await _relatedStallRepository.GetAllAsync(x => x.StallID.Equals(model.ID));
                await _relatedStallRepository.DeleteListAsync(listStallRelated);
                if (input.ListPromotionID != null)
                {
                    foreach (var item in input.ListPromotionID)
                    {
                        var relatedStall = new RelatedStall
                        {
                            NewsID = item,
                            StallID = model.ID
                        };
                        await _relatedStallRepository.AddAsync(relatedStall);
                    }
                }
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteStall(int ID)
        {
            try
            {
                var model = await _stallRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                model.IsActive = SystemParam.ACTIVE_FALSE;
                await _stallRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        
    }
}
