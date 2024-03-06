using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.DTOs.News;

namespace APIProject.Repository
{
    public class RelatedStallRepository:BaseRepository<RelatedStall> ,IRelatedStallRepository
    {
        public RelatedStallRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
