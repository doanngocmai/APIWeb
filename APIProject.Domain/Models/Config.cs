using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Config : BaseModel
    {
        [StringLength(100)]
        public string Key { get; set; }
        [StringLength(100)]
        public string Value { get; set; }
        public long ValueLong { get; set; }
    }
}
