using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Repository
{
    public class QRCodeBillRepository : BaseRepository<QRCodeBill>, IQRCodeBillRepository
    {
        public QRCodeBillRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
