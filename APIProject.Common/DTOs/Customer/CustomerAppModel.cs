using APIProject.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Customer
{
    public class CustomerAppModel
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string DeviceID { get; set; }
        public string Email { get; set; }
        public string QRCode { get; set; }
        public string Avatar { get; set; }
        public string IdentityNumber { get; set; }
        public string Job { get; set; }
        public long Point { get; set; }
        public int? ProvinceID { get; set; }
        public int? DistrictID { get; set; }
        public int? WardID { get; set; }
        public int Role { get; set; }
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public int Status { get; set; } = SystemParam.ACTIVE;
        public DateTime? ExpireDateOTP { get; set; }
        public DateTime? ResetDateOTP { get; set; }
        public string OTP { get; set; }
        public int QtyOTP { get; set; }
        public string Address { get; set; }
    }
}
