using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Stall : BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        public string Code { get; set; }
        public int Floor { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public string LinkWeb { get; set; }
        public string LinkFB { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? Index { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public ICollection<RelatedStall> RelatedStalls { get; set; }
        public ICollection<Bill> Bills { get; set; }
    }
}
