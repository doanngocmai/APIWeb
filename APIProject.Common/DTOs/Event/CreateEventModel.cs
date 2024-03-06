using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Event
{
    public class CreateEventModel
    {
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UrlImage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IsPopup { get; set; }
        public int IsNotify { get; set; }
        public int IsBanner { get; set; }
        public int GiftLimit { get; set; }
        public List<EventGiftModel> ListEventGift { get; set; }
        public List<int> ListRelatedStall { get; set; }
    }

    public class EventGiftModel
    {
        public string Name { get; set; }
        public int GiftID { get; set; }
        public int Quantity { get; set; }
        public int QuantityExchanged { get; set; }
        public int Stock { get; set; }
    }
}
