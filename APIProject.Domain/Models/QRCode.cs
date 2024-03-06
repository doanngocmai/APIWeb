using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class QRCode : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public DateTime DOB { get; set; }
        public int Gender { get; set; }
        public string IdentityNumber { get; set; }
        public string Job { get; set; }
        public long TotalPrice { get; set; }
        public int NewsID { get; set; }
        public News News { get; set; }
        public int EventChannelID { get; set; }
        public EventChannel EventChannel { get; set; }
        public ICollection<QRCodeBill> QRCodeBills { get; set; }


    }
}
