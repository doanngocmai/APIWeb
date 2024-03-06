using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIProject.Domain.Models
{
    public class GiftNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int GiftID { get; set; }
        public Gift Gift { get; set; }
        public int NewsID { get; set; }
        public News News { get; set; }
    }
}
