using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class MemberPointHistory : BaseModel
    {
        public long Point { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public long Balance { get; set; }
        public int? GiftID { get; set; }
        public Gift Gift { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int? EventParticipantID { get; set; }
        public EventParticipant EventParticipant { get; set; }
    }
}
