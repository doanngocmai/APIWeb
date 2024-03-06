using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Customer
{
    public class CustomerEventDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
