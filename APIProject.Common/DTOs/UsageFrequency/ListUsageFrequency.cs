using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace APIProject.Common.DTOs.UsageFrequency
{
    public class ListUsageFrequency
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public double? AverageTime { get; set; }
        public int? SumEventClick { get; set; }
        public int? SumNewsClick { get; set; }
        public List<UsageFrequencyModel> ListJoinDay { get; set; }

    }
    public class UsageFrequencyModel
    {
        public DateTime? UseDate { get; set; }
        public int? UseDuration { get; set; }
        public int? EventClick { get; set; }
        public int? NewsClick { get; set; }
    }
}
