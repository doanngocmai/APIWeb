using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Stall
{
    public class StallModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Floor { get; set; }
        public int? Index { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public int Status { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CreatedDate { get; set; }
    }
}
