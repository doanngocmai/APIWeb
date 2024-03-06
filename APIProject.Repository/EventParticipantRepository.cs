using APIProject.Common.DTOs.Bill;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;

namespace APIProject.Repository
{
    public class EventParticipantRepository:BaseRepository<EventParticipant>, IEventParticipantRepository
    {
        public EventParticipantRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IPagedList<ListBillModel>> GetEventParticipantDetail(int page, int limit, int ID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from ev in DbContext.EventParticipants 
                                 join b in DbContext.Bills on ev.ID equals b.EventParticipantID
                                 where ev.ID.Equals(ID)
                                 && ev.IsActive.Equals(SystemParam.ACTIVE)
                                 select new ListBillModel
                                 {
                                     ID = b.ID,
                                     Code = b.Code,
                                     Image = b.ImageUrl,
                                     Price = b.Price,
                                     StallName = b.Stall.Name,
                                 }).OrderByDescending(p => p.ID).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
