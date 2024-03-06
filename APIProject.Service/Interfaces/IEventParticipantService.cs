using APIProject.Common.DTOs.EventParticipant;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IEventParticipantService
    {
        Task<JsonResultModel> CreateQRCode(CreateQRCodeModel input);
        Task<JsonResultModel> ScanQRCode(ScanQRCodeModel input);
        Task<JsonResultModel> InputCustomerInfo(EventParticipantModel input, int StaffID);
        Task<JsonResultModel> GetListEvent();
        Task<JsonResultModel> GetListEventChannel();
        Task<JsonResultModel> GetListStall();
        Task<JsonResultModel> GetListEventGift(int eventID);
        Task<JsonResultModel> GetEventParticipantDetail(int page, int limit, int ID);
    }
}
