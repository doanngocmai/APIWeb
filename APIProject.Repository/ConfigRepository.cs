using APIProject.Domain.Models;
using APIProject.Domain;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.DTOs.Config;
using APIProject.Common.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace APIProject.Repository
{
    public class ConfigRepository : BaseRepository<Config>, IConfigRepository
    {
        public ConfigRepository(ApplicationDbContext dbContext) : base(dbContext) { }

 
    }
}
