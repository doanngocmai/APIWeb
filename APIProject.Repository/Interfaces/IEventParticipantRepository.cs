﻿using APIProject.Common.DTOs.Bill;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IEventParticipantRepository : IRepository<EventParticipant>
    {
        Task<IPagedList<ListBillModel>> GetEventParticipantDetail(int page, int limit, int ID);
    }
}
