using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class GiftNewsRepository : BaseRepository<GiftNews>, IGiftNewsRepository
    {
        public GiftNewsRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
