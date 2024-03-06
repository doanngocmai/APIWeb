using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.MenberPointHistory
{
    public class MemberChangeHistoryModel
    {
        public int ID { get; set; }
        public string GiftName { get; set; }
        public int? GiftID { get; set; }
        public int Type { get; set; }
        public long Point { get; set; }
        public long Balance { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
