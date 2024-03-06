using APIProject.Common.DTOs.EventParticipant;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IQRCodeRepository : IRepository<QRCode>
    {

        Task<ScanQRCodeOutputModel> ScanQRCode(string QRcode);
    }
}
