using APIProject.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Customer : BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public string IdentityNumber { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        public string Token { get; set; }
        public string Avatar { get; set; }
        public int Role { get; set; }
        public string OTP { get; set; }
        public int? QtyOTP { get; set; }
        public DateTime? ResetDateOTP { get; set; }
        public DateTime? ExpiredDateOTP { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? OriginCustomer { get; set; }
        public int? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int? AgeType { get; set; }
        public string Address { get; set; }
        public int Status { get; set; } = SystemParam.ACTIVE;
        public long Point { get; set; }
        public string DeviceID { get; set; }
        public string Job { get; set; }
        public int? WardID { get; set; }
        public Ward Ward { get; set; }
        public int? DistrictID { get; set; }
        public District District { get; set; }
        public int? ProvinceID { get; set; }
        public Province Province { get; set; }
        public ICollection<MemberPointHistory> MemberPoints { get; set; }
        public ICollection<EventParticipant> EventParticipants { get; set; }

    }
}
