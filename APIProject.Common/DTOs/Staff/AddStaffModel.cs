using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Staff
{
    public class AddStaffModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int? WardID { get; set; }
        public int? DistrictID { get; set; }
        public int? ProvinceID { get; set; }
        public int? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
    }

    public class UpdateStaffModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? WardID { get; set; }
        public int? DistrictID { get; set; }
        public int? ProvinceID { get; set; }
        public int? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
    }
}
