using APIProject.Common.DTOs.Config;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IConfigService
    {
        Task<JsonResultModel> GetLinkSurvery();
        Task<JsonResultModel> GetContact();
        Task<JsonResultModel> GetEventInfo();

        Task<JsonResultModel> UpdateLinkSurvery(string linkSurvery);
        Task<JsonResultModel> UpdateContact(string linkHotLine, string linkWebsite, string linkFacebook);
        Task<JsonResultModel> UpdateEventInfo(long pointAdd, long orderValue);

        Task<JsonResultModel> GetContactInfo();
    }
}
