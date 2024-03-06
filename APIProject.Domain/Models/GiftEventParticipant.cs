using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class GiftEventParticipant : BaseModel
    {
        public int EventParticipantID { get; set; }
        public EventParticipant EventParticipant { get; set; }
        public int GiftID { get; set; }
        public Gift Gift { get; set; }
    }
}
