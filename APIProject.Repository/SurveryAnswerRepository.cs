using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class SurveryAnswerRepository : BaseRepository<SurveyAnswer>, ISurveyAnswerRepository
    {
        public SurveryAnswerRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
