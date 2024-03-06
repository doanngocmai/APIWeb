using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Requests : BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        public int Type { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int GiftID { get; set; }
        public Gift Gift { get; set; }
    }
}
