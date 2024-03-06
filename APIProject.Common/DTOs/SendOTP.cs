using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs
{
    public class SendOTP
    {
        public string to { get; set; }
        public string telco { get; set; } = "";
        public string orderCode { get; set; } = "";
        public string packageCode { get; set; } = "";
        public int type { get; set; } = 1;
        public string from { get; set; }
        public string message { get; set; }
        public string scheduled { get; set; } = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        public string requestId { get; set; } = "";
        public int useUnicode { get; set; } = 0;
        public int maxMt { get; set; } = 0;
        public object ext { get; set; } = new object();
    }
}
