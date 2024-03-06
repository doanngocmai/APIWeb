using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.EventParticipant
{
    public class CreateQRCodeModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public long TotalPrice { get; set; }
        public int EventID { get; set; }
        public int EventChannelID { get; set; }
        public List<QRCodeBillModel> ListBill { get; set; }
    }
    public class QRCodeBillModel
    {
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public long Price { get; set; }
        public int StallID { get; set; }
    }
}
