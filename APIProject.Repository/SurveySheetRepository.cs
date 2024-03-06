using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class SurveySheetRepository : BaseRepository<SurveySheet>, ISurveySheetRepository
    {
        public SurveySheetRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
