using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Gift
{
    public class ListGiftModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public int Type { get; set; }
        public int Quantity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
    }
    public class ListGiftNews
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public string Expiry { get; set; }
    }
       

}
