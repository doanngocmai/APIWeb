using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Gift
{
    public class ListChangeGiftModel
    {
        public int ID { get; set; }
        public int? GiftID { get; set; }
        public string GiftName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int Type { get; set; }
        public long Point { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
