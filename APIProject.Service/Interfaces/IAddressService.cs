using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IAddressService
    {
        Task<JsonResultModel> GetListProvice();
        Task<JsonResultModel> GetListDistrict(int provinceID);
        Task<JsonResultModel> GetListWard(int districtID);
    }
}
