using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.MenberPointHistory
{
    public class PointHistoryModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public long Balance { get; set; }
        public long Point { get; set; }
        public string ImageUrl { get; set; }
        public string CreatedDate { get; set; }
    }
}
