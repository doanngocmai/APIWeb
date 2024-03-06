using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class GiftCodeQRRepository: BaseRepository<GiftCodeQR>, IGiftCodeQRRepository
    {
        public GiftCodeQRRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
