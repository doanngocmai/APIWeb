using APIProject.Common.DTOs.EventGift;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class GiftEventRepository : BaseRepository<GiftEvent>, IGiftEventRepository
    {
        public GiftEventRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<EventGiftModel>> GetListEventGiftModel(int eventID)
        {
            try
            {
                var model = await (from g in DbContext.Gifts
                                   join ge in DbContext.GiftEvents on g.ID equals ge.GiftID
                                   where g.IsActive.Equals(SystemParam.ACTIVE) && ge.IsActive.Equals(SystemParam.ACTIVE)
                                   && ge.NewsID.Equals(eventID) && ge.Quantity > 0
                                   orderby ge.CreatedDate descending
                                   select new EventGiftModel
                                   {
                                       ID = g.ID,
                                       Name = g.Name,
                                       Quantity = ge.Quantity,
                                       ImageUrl = g.UrlImage
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
