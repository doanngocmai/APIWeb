using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Category:BaseModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public ICollection<Stall> Stalls { get; set; }
    }
}
