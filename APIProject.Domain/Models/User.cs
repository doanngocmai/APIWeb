using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class User : BaseModel
    {
        [StringLength(150)]
        public string Username { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int Role { get; set; }
        public int Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }

    }
}
