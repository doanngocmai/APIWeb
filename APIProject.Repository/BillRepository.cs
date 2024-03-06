using APIProject.Common.DTOs.Bill;
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
    public class BillRepository:BaseRepository<Bill>, IBillRepository
    {
        public BillRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    }
}
