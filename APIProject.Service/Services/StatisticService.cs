using APIProject.Common.DTOs.Customer;
using APIProject.Common.Utils;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
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
    public class StatisticService : IStatisticService
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IHub _sentryHub;

        public StatisticService(ICustomerRepository customerRepository, INewsRepository newsRepository, IHub hub)
        {
            _customerRepository = customerRepository;
            _newsRepository = newsRepository;
            _sentryHub = hub;
        }

        public async Task<JsonResultModel> GetListSameFilter(int? Type)
        {
            try
            {
                var ListCus = new List<CustomersSamePeriodModel>();
                var ListCusCompare = new List<CustomersSamePeriodModelCompare>();
                DateTime fromDate;
                DateTime toDate;
                if (Type == SystemParam.TYPE_SAME_FILTER_WEEK)
                {
                    var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1);
                    var firstDayOfMonthCompare = firstDayOfMonth.AddMonths(-1);
                    var lastDayOfMonthCompare = firstDayOfMonthCompare.AddMonths(1);
                    for (int i = 1; i <= 5; i++)
                    {
                        fromDate = firstDayOfMonth.AddDays(7 * (i - 1));
                        toDate = i < 5 ? firstDayOfMonth.AddDays(7 * i) : lastDayOfMonth;
                        CustomersSamePeriodModel customerCompare = new CustomersSamePeriodModel()
                        {
                            Time = "Tuần " + i,
                            Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCus.Add(customerCompare);

                        fromDate = firstDayOfMonthCompare.AddDays(7 * (i - 1));
                        toDate = i < 5 ? firstDayOfMonthCompare.AddDays(7 * i) : lastDayOfMonthCompare;
                        CustomersSamePeriodModelCompare CustomerWeekCompares = new CustomersSamePeriodModelCompare()
                        {
                            Time = "Tuần " + i,
                            AmountCompare = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCusCompare.Add(CustomerWeekCompares);
                    };
                }
                else if (Type == SystemParam.TYPE_SAME_FILTER_MONTH)
                {
                    var firstDayOfYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var firstDayOfYearCompare = firstDayOfYear.AddYears(-1);
                    for (int i = 1; i <= 12; i++)
                    {
                        fromDate = firstDayOfYear.AddMonths(i - 1);
                        toDate = firstDayOfYear.AddMonths(i);
                        CustomersSamePeriodModel customerCompare = new CustomersSamePeriodModel()
                        {
                            Time = "Tháng " + i,
                            Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCus.Add(customerCompare);

                        fromDate = firstDayOfYearCompare.AddMonths(i - 1);
                        toDate = firstDayOfYearCompare.AddMonths(i);
                        CustomersSamePeriodModelCompare CustomerWeekCompares = new CustomersSamePeriodModelCompare()
                        {
                            Time = "Tháng " + i,
                            AmountCompare = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCusCompare.Add(CustomerWeekCompares);
                    };
                }
                else if (Type == SystemParam.TYPE_SAME_FILTER_QUARTER)
                {
                    var firstDayOfYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var firstDayOfYearCompare = firstDayOfYear.AddYears(-1);
                    for (int i = 1; i <= 4; i++)
                    {
                        fromDate = firstDayOfYear.AddMonths(3 * (i - 1));
                        toDate = firstDayOfYear.AddMonths(3 * i);
                        CustomersSamePeriodModel customerCompare = new CustomersSamePeriodModel()
                        {
                            Time = "Quý " + i,
                            Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCus.Add(customerCompare);

                        fromDate = firstDayOfYearCompare.AddMonths(3 * (i - 1));
                        toDate = firstDayOfYearCompare.AddMonths(3 * i);
                        CustomersSamePeriodModelCompare CustomerWeekCompares = new CustomersSamePeriodModelCompare()
                        {
                            Time = "Quý " + i,
                            AmountCompare = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCusCompare.Add(CustomerWeekCompares);
                    };
                }
                else if (Type == SystemParam.TYPE_SAME_FILTER_YEAR)
                {
                    var firstDayOfYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var firstDayOfYearCompare = firstDayOfYear.AddYears(-1);
                    fromDate = firstDayOfYear;
                    toDate = firstDayOfYear.AddYears(1);
                    CustomersSamePeriodModel customerCompare = new CustomersSamePeriodModel()
                    {
                        Time = "Năm " + firstDayOfYear.Year,
                        Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                    };
                    ListCus.Add(customerCompare);

                    fromDate = firstDayOfYearCompare;
                    toDate = firstDayOfYearCompare.AddYears(1);
                    CustomersSamePeriodModelCompare CustomerWeekCompares = new CustomersSamePeriodModelCompare()
                    {
                        Time = "Năm " + firstDayOfYear.Year,
                        AmountCompare = await _customerRepository.CountEventParticipant(fromDate, toDate)
                    };
                    ListCusCompare.Add(CustomerWeekCompares);
                }

                var Model = new CustomersStatisticsParticipatingEvent
                {
                    ListCustomerWeek = ListCus,
                    ListCustomerWeekCompare = ListCusCompare
                };
                return JsonResponse.Success(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResultModel> FilterEventParticipant(string Fromdate, string Todate)
        {
            try
            {
                var fd = Util.ConvertFromDate(Fromdate);
                var td = Util.ConvertToDate(Todate);
                DateTime fromDate;
                DateTime toDate;
                double totalDay = (td.Value - fd.Value).TotalDays;
                var ListCus = new List<CustomersSamePeriodModel>();
                if (fd == null || td == null)
                {
                    fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    toDate = fromDate.AddDays(1);
                    var Model = new CustomersSamePeriodModel
                    {
                        Time = fromDate.ToString("dd/MM/yyyy"),
                        Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                    };
                    ListCus.Add(Model);
                }
                else if (totalDay <= 1)
                {
                    fromDate = fd.Value;
                    toDate = fromDate.AddDays(1);
                    var Model = new CustomersSamePeriodModel
                    {
                        Time = fromDate.ToString("dd/MM/yyyy"),
                        Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                    };
                    ListCus.Add(Model);
                }
                else if (totalDay <= 7)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new CustomersSamePeriodModel
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCus.Add(Model);
                    }
                }
                else if (totalDay <= 32)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        fromDate = fd.Value.AddDays(7 * (i - 1));
                        toDate = i < 5 ? fd.Value.AddDays(7 * i) : td.Value;
                        var Model = new CustomersSamePeriodModel
                        {
                            Time = "Tuần " + i,
                            Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCus.Add(Model);
                    }
                }
                else if (totalDay <= 367)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        fromDate = fd.Value.AddMonths(i - 1);
                        toDate = fd.Value.AddMonths(i);
                        var Model = new CustomersSamePeriodModel
                        {
                            Time = "Tháng " + i,
                            Amount = await _customerRepository.CountEventParticipant(fromDate, toDate)
                        };
                        ListCus.Add(Model);
                    }
                }
                return JsonResponse.Success(ListCus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> GetCustomerCampaign(int ID)
        {
            try
            {
                var ListCus = new List<CustomersSamePeriodModel>();
                var model = await _newsRepository.GetFirstOrDefaultAsync(c => c.ID.Equals(ID));
                if (model == null || model.StartDate == null || model.EndDate == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_MISSING_DATE, SystemParam.MESSAGE_MISSING_DATE);
                }
                int CountDay = (int)(model.EndDate.Value - model.StartDate.Value).TotalDays;
                DateTime time;
                for (int i = 0; i < CountDay; i++)
                {
                    time = model.StartDate.Value.AddDays(i);
                    var Model = new CustomersSamePeriodModel
                    {
                        Time = time.ToString("dd/MM/yyyy"),
                        Amount = await _customerRepository.CountCustomerCampaign(ID, time)
                    };
                    ListCus.Add(Model);
                }
                return JsonResponse.Success(ListCus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResultModel> GetCountGiftBills(string Fromdate, string Todate, int? EventID)
        {
            try
            {
                var fd = Util.ConvertFromDate(Fromdate);
                var td = Util.ConvertToDate(Todate);
                DateTime fromDate;
                DateTime toDate;
                double totalDay = (td.Value - fd.Value).TotalDays;

                var ListBill = new List<ExchangeGiftProgram>();
                if (EventID != null)
                {
                    var model = await _newsRepository.GetFirstOrDefaultAsync(c => c.ID.Equals(EventID.Value));
                    if (model == null || model.StartDate == null || model.EndDate == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_MISSING_DATE, SystemParam.MESSAGE_MISSING_DATE);
                    }
                    int CountDay = (int)(model.EndDate.Value - model.StartDate.Value).TotalDays;
                    DateTime time;
                    for (int i = 0; i < CountDay; i++)
                    {
                        time = model.StartDate.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = time.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.CountTotalPriceCampaign(EventID.Value, time)
                        };
                        ListBill.Add(Model);
                    }
                }
                if (fd == null || td == null)
                {
                    fd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 7)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 32)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        fromDate = fd.Value.AddDays(7 * (i - 1));
                        toDate = i < 5 ? fd.Value.AddDays(7 * i) : td.Value;
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tuần " + i,
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 367)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        fromDate = fd.Value.AddMonths(i - 1);
                        toDate = fd.Value.AddMonths(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tháng " + i,
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                return JsonResponse.Success(ListBill);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<ExchangeGiftProgram>> GetCountGiftBills(string Fromdate, string Todate)
        {
            try
            {
                var fd = Util.ConvertFromDate(Fromdate);
                var td = Util.ConvertToDate(Todate);
                DateTime fromDate;
                DateTime toDate;
                double totalDay = (td.Value - fd.Value).TotalDays;

                var ListBill = new List<ExchangeGiftProgram>();
                if (fd == null || td == null)
                {
                    fd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 7)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 32)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        fromDate = fd.Value.AddDays(7 * (i - 1));
                        toDate = i < 5 ? fd.Value.AddDays(7 * i) : td.Value;
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tuần " + i,
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 367)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        fromDate = fd.Value.AddMonths(i - 1);
                        toDate = fd.Value.AddMonths(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tháng " + i,
                            Values = await _customerRepository.CountGiftBills(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                return ListBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResultModel> ExportExcelCountGiftBills(string Fromdate, string Todate, IWebHostEnvironment webHostEnvironment, HttpContext context)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, "Template");
                var fullPath = Path.Combine(pathToSave, SystemParam.EXPORT_SAMPLE_GH_TSHDDQTTT);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var newFileName = SystemParam.EXPORT_SAMPLE_GH_TSHDDQTTT_NEW + DateTime.Now.ToString("ssddMMyyyy") + ".xlsx";
                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExcelPackage pack = new ExcelPackage(stream);
                    ExcelWorkbook workbook = pack.Workbook;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                    int row = 3;
                    var list = await GetCountGiftBills(Fromdate, Todate);
                    int stt = 1;
                    foreach (var item in list)
                    {
                        sheet.Cells[row, 1].Value = stt;
                        sheet.Cells[row, 2].Value = item.Time;
                        sheet.Cells[row, 3].Value = @String.Format("{0:0,0}", item.Values);
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
        public async Task<JsonResultModel> GetTotalGiftBills(string Fromdate, string Todate, int? EventID)
        {
            try
            {
                var fd = Util.ConvertFromDate(Fromdate);
                var td = Util.ConvertFromDate(Todate);
                DateTime fromDate;
                DateTime toDate;
                double totalDay = (td.Value - fd.Value).TotalDays;
                var ListBill = new List<ExchangeGiftProgram>();
                if (EventID != null)
                {
                    var model = await _newsRepository.GetFirstOrDefaultAsync(c => c.ID.Equals(EventID.Value));
                    if (model == null || model.StartDate == null || model.EndDate == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_MISSING_DATE, SystemParam.MESSAGE_MISSING_DATE);
                    }
                    int CountDay = (int)(model.EndDate.Value - model.StartDate.Value).TotalDays;
                    DateTime time;
                    for (int i = 0; i < CountDay; i++)
                    {
                        time = model.StartDate.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = time.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.CountTotalPriceCampaign(EventID.Value, time)
                        };
                        ListBill.Add(Model);
                    }
                }
                if (fd == null || td == null)
                {
                    fd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 7)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 32)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        fromDate = fd.Value.AddDays(7 * (i - 1));
                        toDate = i < 5 ? fd.Value.AddDays(7 * i) : td.Value;
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tuần " + i,
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 367)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        fromDate = fd.Value.AddMonths(i - 1);
                        toDate = fd.Value.AddMonths(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tháng " + i,
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                return JsonResponse.Success(ListBill);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<ExchangeGiftProgram>> GetTotalGiftBills(string Fromdate, string Todate)
        {
            try
            {
                var fd = Util.ConvertFromDate(Fromdate);
                var td = Util.ConvertFromDate(Todate);
                DateTime fromDate;
                DateTime toDate;
                double totalDay = (td.Value - fd.Value).TotalDays;
                var ListBill = new List<ExchangeGiftProgram>();
                if (fd == null || td == null)
                {
                    fd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 7)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        fromDate = fd.Value.AddDays(i - 1);
                        toDate = fd.Value.AddDays(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = fromDate.ToString("dd/MM/yyyy"),
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 32)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        fromDate = fd.Value.AddDays(7 * (i - 1));
                        toDate = i < 5 ? fd.Value.AddDays(7 * i) : td.Value;
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tuần " + i,
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                else if (totalDay <= 367)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        fromDate = fd.Value.AddMonths(i - 1);
                        toDate = fd.Value.AddMonths(i);
                        var Model = new ExchangeGiftProgram
                        {
                            Time = "Tháng " + i,
                            Values = await _customerRepository.SumBillAmount(fromDate, toDate)
                        };
                        ListBill.Add(Model);
                    }
                }
                return ListBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> ExportExcelTotalGiftBills(string Fromdate, string Todate, IWebHostEnvironment webHostEnvironment, HttpContext context)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, "Template");
                var fullPath = Path.Combine(pathToSave, SystemParam.EXPORT_SAMPLE_GH_TGTHDDQ);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var newFileName = SystemParam.EXPORT_SAMPLE_GH_TGTHDDQ_NEW + DateTime.Now.ToString("ssddMMyyyy") + ".xlsx";
                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExcelPackage pack = new ExcelPackage(stream);
                    ExcelWorkbook workbook = pack.Workbook;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];
                    int row = 3;
                    var list = await GetTotalGiftBills(Fromdate, Todate);
                    int stt = 1;
                    foreach (var item in list)
                    {
                        sheet.Cells[row, 1].Value = stt;
                        sheet.Cells[row, 2].Value = item.Time;
                        sheet.Cells[row, 3].Value = @String.Format("{0:0,0}", item.Values);
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
