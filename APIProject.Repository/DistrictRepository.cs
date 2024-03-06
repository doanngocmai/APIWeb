using APIProject.Common.DTOs.Address;
using APIProject.Domain;
using APIProject.Domain.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIProject.Repository.Interfaces;

namespace APIProject.Repository
{
    public class DistrictRepository :BaseRepository<District>, IDistrictRepository
    {
        public DistrictRepository (ApplicationDbContext dbContext ) : base(dbContext) { }

        public async Task<IList<DistrictModel>> GetListDistrict(int ProvinceID)
        {
            try
            {
                var model = await (from d in DbContext.Districts
                                   where d.ProvinceID == ProvinceID
                                   orderby d.Name
                                   select new DistrictModel
                                   {
                                       Code = d.Code,
                                       Name = d.Name,
                                       Type = d.Type,
                                       ProvinceID = d.ProvinceID
                                   }).ToListAsync();
                return model;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
