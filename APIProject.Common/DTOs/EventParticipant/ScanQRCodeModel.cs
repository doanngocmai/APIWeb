using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.EventParticipant
{
    public class ScanQRCodeModel
    {
        public string Code { get; set; }
    }
    public class ScanQRCodeOutputModel : CreateQRCodeModel
    {
        public int ID { get; set; }
    }
}
