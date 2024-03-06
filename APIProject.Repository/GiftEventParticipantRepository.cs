using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class GiftEventParticipantRepository : BaseRepository<GiftEventParticipant>, IGiftEventParticipantRepository
    {
        public GiftEventParticipantRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
