using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Bill
{
    public class SumBillCategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double AverageBillPrice { get; set; }
    }
}
