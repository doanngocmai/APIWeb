using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Gift
{
    public class VoucherModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public string UrlImage { get; set; }
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
    }
    public class MyVoucherModel : VoucherModel
    {
        public string Code { get; set; }
    }
}
