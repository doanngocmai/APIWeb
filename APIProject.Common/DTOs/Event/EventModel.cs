using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Event
{
    public class EventModel 
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public int TotalGift { get; set; }
    }
}
