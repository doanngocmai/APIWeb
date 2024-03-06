using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Config
{
    public class ConfigSurveyModel
    {    
        public string linkSurvery { get; set; }      
    }
    public class ConfigContactModel
    {
        public string LinkWebsite { get; set; }
        public string LinkHotline { get; set; }
        public string LinkHotFacebook { get; set; }    
    }
    public class ConfigEventInfoModel
    {       
        public long PointAdd { get; set; }
        public long OrderValue { get; set; }

    }
}
