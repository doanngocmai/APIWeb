using APIProject.Domain;
using APIProject.Domain.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIProject.Common.DTOs.Address;
using APIProject.Repository.Interfaces;

namespace APIProject.Repository
{
    public class ProvinceRepository : BaseRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IList<ProvinceModel>> GetListProvice()
        {
            try
            {
                var model = await (from p in DbContext.Provinces
                                   orderby p.Name
                                   select new ProvinceModel
                                   {
                                       Code = p.Code,
                                       Name = p.Name,
                                       Type = p.Type,
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
