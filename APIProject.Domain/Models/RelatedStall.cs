using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIProject.Domain.Models
{
    public class RelatedStall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int StallID { get; set; }
        public Stall Stall { get; set; }
        public int NewsID { get; set; }
        public News News { get; set; }
    }
}
