using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Bill
{
    public class ListBillModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
        public int Status { get; set; }
        public string StallName { get; set; }
    }
}
