using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Bill
{
    public class BillModel
    {
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public int StallID { get; set; }
    }
}
