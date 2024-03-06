using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Gift:BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        public int Point { get; set; }
        public string UrlImage { get; set; }
        public int Type { get; set; }
        public int Number { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int CreateByID { get; set; }
        public ICollection<GiftEvent> GiftEventChannel { get; set; }
        public ICollection<GiftCodeQR> GiftCodeQRs { get; set; }
        public ICollection<MemberPointHistory> MemberPointHistories { get; set; }
    }
}
