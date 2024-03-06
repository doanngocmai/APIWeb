using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Gift
{
    public class GitfDetailModelApp
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public int Type { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Code { get; set; }
    }
 
}

