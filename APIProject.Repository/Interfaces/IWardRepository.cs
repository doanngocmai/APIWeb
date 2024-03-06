using APIProject.Common.DTOs.Address;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IWardRepository:IRepository<Ward>
    {
        Task<IList<WardModel>> GetListWard(int districtID);
    }
}
