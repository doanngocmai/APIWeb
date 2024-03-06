using APIProject.Common.DTOs.Category;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStallRepository _stallRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IHub sentryHub, IStallRepository stallRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _stallRepository = stallRepository;
        }

        public async Task<JsonResultModel> GetListCategory(int page, int limit)
        {
            try
            {
                var model = await _categoryRepository.GetListCategory(page, limit);
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

        public async Task<JsonResultModel> CreateCategory(CreateCategoryModel input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Name))
                    return JsonResponse.Error(SystemParam.ERROR_CATEGORY_FIELDS_INVALID, SystemParam.MESSAGE_CATEGORY_FIELDS_INVALID);
                Category ca = new Category()
                {
                    Name = input.Name,
                    ImageUrl = input.ImageUrl,
                    Status = SystemParam.ACTIVE,
                };
                await _categoryRepository.AddAsync(ca);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateCategory(CategoryModel input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Name))
                    return JsonResponse.Error(SystemParam.ERROR_CATEGORY_FIELDS_INVALID, SystemParam.MESSAGE_CATEGORY_FIELDS_INVALID);
                var model = await _categoryRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                _mapper.Map(input, model);
                await _categoryRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteCategory(int ID)
        {
            try
            {
                var model = await _categoryRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null) return JsonResponse.Error(SystemParam.ERROR_CATEGORY_NOT_FOUND, SystemParam.MESSAGE_CATEGORY_NOT_FOUND);
                var stall = await _stallRepository.GetFirstOrDefaultAsync(x => x.CategoryID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (stall != null) return JsonResponse.Error(SystemParam.ERROR_CATEGORY_EXIST_STALL, SystemParam.MESSAGE_CATEGORY_EXIST_STALL);
                model.IsActive = SystemParam.ACTIVE_FALSE;
                await _categoryRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListCategory()
        {
            try
            {
                var model = await _categoryRepository.GetListCategory();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetSumBillCategory(int type, int? EventID)
        {
            try
            {
                var model = await _categoryRepository.GetSumBillCategory(type, EventID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ExportExcelSumBillCategory(IWebHostEnvironment webHostEnvironment, HttpContext context)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, "Template");
                var fullPath = Path.Combine(pathToSave, SystemParam.EXPORT_SAMPLE_SUM_BILL_CATEGORY);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var newFileName = SystemParam.EXPORT_SAMPLE_SUM_BILL_CATEGORY_NEW + DateTime.Now.ToString("ssddMMyyyy") + ".xlsx";
                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExcelPackage pack = new ExcelPackage(stream);
                    ExcelWorkbook workbook = pack.Workbook;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                    int row = 2;
                    var list = await _categoryRepository.GetAllSumBillCategory();
                    int stt = 1;
                    foreach (var item in list)
                    {
                        sheet.Cells[row, 1].Value = stt;
                        sheet.Cells[row, 2].Value = item.Name;
                        sheet.Cells[row, 3].Value = @String.Format("{0:0,00}", item.AverageBillPrice);
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
