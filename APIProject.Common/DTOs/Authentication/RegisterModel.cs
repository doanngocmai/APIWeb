using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Authentication
{
   public class RegisterModel
    {
        public string Phone { get; set; }
        public string DeviceID { get; set; }
        public string OTP { get; set; }
    }
}
