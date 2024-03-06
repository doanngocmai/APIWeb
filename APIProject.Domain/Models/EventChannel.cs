using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class EventChannel:BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
