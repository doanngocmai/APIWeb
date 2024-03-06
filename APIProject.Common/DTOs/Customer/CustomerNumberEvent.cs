using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace APIProject.Common.DTOs.Customer
{
    public class CustomerNumberEvent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
