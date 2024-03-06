using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class GiftEvent : BaseModel
    {
        public int GiftID { get; set; }
        public int NewsID { get; set; }
        public int QuantityExchanged { get; set; }
        public int Quantity { get; set; }
        public News News { get; set; }
        public Gift Gift { get; set; }
    }
}
