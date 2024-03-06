using APIProject.Common.DTOs.Address;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class WardRepository : BaseRepository<Ward>, IWardRepository
    {
        public WardRepository (ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IList<WardModel>> GetListWard(int districtID)
        {
            try
            {
                var model = await (from d in DbContext.Wards
                                   where d.DistrictID == districtID
                                   orderby d.Name
                                   select new WardModel
                                   {
                                       Code = d.ID,
                                       Name = d.Name,
                                       Type = d.Type,
                                       DistrictID = d.DistrictID,
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
