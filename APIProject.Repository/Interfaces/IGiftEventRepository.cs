using APIProject.Common.DTOs.EventGift;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IGiftEventRepository : IRepository<GiftEvent>
    {

        Task<List<EventGiftModel>> GetListEventGiftModel(int eventID);
    }
}
