using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Gift
{
    public class CreateGiftModel
    {
        public string Name { get; set; }
        public int? Point { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public int? Number { get; set; }
        public int? Index { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class UpdateGiftModel : CreateGiftModel
    {
        public int ID { get; set; }
    }
}
