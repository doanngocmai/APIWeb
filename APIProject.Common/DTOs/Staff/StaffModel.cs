using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Staff
{
    public class StaffModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ProvinceName { get; set; }
        public int? ProvinceID { get; set; }
        public string DistrictName { get; set; }
        public int? DistrictID { get; set; }
        public string WardName { get; set; }
        public int? WardID { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DOB { get; set; }
    }
}
