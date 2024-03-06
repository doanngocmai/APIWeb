using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class EventParticipant:BaseModel
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public string Job { get; set; }
        public string IdentityCard { get; set; }
        public long TotalMoney { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int NewsID { get; set; }
        public News News { get; set; }
        public int EventChannelID { get; set; }
        public EventChannel EventChannel { get; set; }
        public int? StaffID { get; set; }
        public ICollection<Bill> Bills { get; set; }
        public ICollection<MemberPointHistory> MemberPointHistories { get; set; }
        public ICollection<GiftEventParticipant> GiftEventParticipants { get; set; }
    }
}
