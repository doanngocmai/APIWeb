using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using APIProject.Common.DTOs.EventChannel;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class EventChannelRepository : BaseRepository<EventChannel>, IEventChannelRepository
    {
        public EventChannelRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<EventChannelModel>> GetListEventChannel()
        {
            try
            {
                var model = await (from e in DbContext.EventChannels
                                   where e.IsActive.Equals(SystemParam.ACTIVE)
                                   orderby e.ID
                                   select new EventChannelModel
                                   {
                                       ID = e.ID,
                                       Name = e.Name
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
