using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Notification : BaseModel
    {
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int IsAdmin { get; set; }
        public int? NewsID { get; set; }
        public News News { get; set; }
        public int Viewed { get; set; }
    }
}
