using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Bill:BaseModel
    {
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public long Price { get; set; }
        public int EventParticipantID { get; set; }
        public EventParticipant EventParticipant { get; set; }
        public int StallID { get; set; }
        public Stall Stall { get; set; }
    }
}
