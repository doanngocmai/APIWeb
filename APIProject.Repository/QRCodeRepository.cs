using APIProject.Common.DTOs.EventParticipant;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class QRCodeRepository : BaseRepository<QRCode>, IQRCodeRepository
    {
        public QRCodeRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public async Task<ScanQRCodeOutputModel> ScanQRCode(string QRcode)
        {
            try
            {
                var model = await (from c in DbContext.QRCodes
                                   where c.IsActive.Equals(SystemParam.ACTIVE) && c.Code.Equals(QRcode)
                                   select new ScanQRCodeOutputModel
                                   {
                                       Name = c.Name,
                                       Phone = c.Phone,
                                       ProvinceID = c.ProvinceID,
                                       DistrictID = c.DistrictID,
                                       WardID = c.WardID,
                                       Gender = c.Gender,
                                       EventChannelID = c.EventChannelID,
                                       EventID = c.NewsID,
                                       ID = c.ID,
                                       TotalPrice = c.TotalPrice,
                                       ListBill = c.QRCodeBills.Select(x => new QRCodeBillModel
                                       {
                                           StallID = x.StallID,
                                           Code = x.Code,
                                           ImageUrl = x.ImageUrl,
                                           Price = x.Price
                                       }).ToList()
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
