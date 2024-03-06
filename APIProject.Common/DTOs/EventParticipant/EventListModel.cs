using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.EventParticipant
{
    public class EventListModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? GiftLimit { get; set; }
    }
}
