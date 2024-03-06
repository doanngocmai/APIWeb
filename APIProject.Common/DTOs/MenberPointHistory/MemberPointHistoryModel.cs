using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.MenberPointHistory
{
    public class MemberPointHistoryModel
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public string StaffName { get; set; }
        public long CountBill { get; set; }
        public long TotalMoney { get; set; }
        public long Point { get; set; }
        public long Balance { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
