using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Address
{
    public class ProvinceModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class DistrictModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int ProvinceID { get; set; }
        public string Type { get; set; }
    }
    public class WardModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int DistrictID { get; set; }
        public string Type { get; set; }
    }
}
