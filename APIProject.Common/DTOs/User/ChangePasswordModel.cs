using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Users
{
    public class ChangePasswordWebModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ChangePasswordOTPModel
    {
        public string IdToken { get; set; }
        public string Password { get; set; }
    }
    public class ChangePasswordModel
    {
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
    }
}
