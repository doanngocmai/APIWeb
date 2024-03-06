using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Notification
{
    public class ListNotificationModel
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int? NewsID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int Viewed { get; set; }
        public string CreateDate { get; set; }
    }
}
