﻿using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Customer
{
    public class CustomerQRCodeAppModel
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public int? ProvinceID { get; set; }
        public int? DistrictID { get; set; }
        public int? WardID { get; set; }
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public string Job { get; set; }
        public string IdentityNumber { get; set; }
    }
}