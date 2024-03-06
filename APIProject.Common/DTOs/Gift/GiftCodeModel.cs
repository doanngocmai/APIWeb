using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Gift
{
    public class CreateGiftCodeModel
    {
        public string Code { get; set; }
        public int GiftID { get; set; }
    }

    public class UpdateGiftCodeModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
    }
}
