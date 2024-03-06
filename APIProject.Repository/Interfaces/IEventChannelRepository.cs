using APIProject.Common.DTOs.EventChannel;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IEventChannelRepository : IRepository<EventChannel>
    {
        Task<List<EventChannelModel>> GetListEventChannel();
    }
}
