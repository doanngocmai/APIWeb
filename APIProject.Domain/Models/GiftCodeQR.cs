using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIProject.Domain.Models
{
    public class GiftCodeQR : BaseModel
    {
        [StringLength(50)]
        public string Code { get; set; }
        public int Status { get; set; }
        public int GiftID { get; set; }
        public int? CustomerID { get; set; }
        public Customer Customer { get; set; }
        public Gift Gift { get; set; }

    }
}
