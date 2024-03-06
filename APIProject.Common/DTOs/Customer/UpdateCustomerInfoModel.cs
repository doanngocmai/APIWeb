using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Customer
{
    public class UpdateCustomerInfoModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        //public string IdentityNumber { get; set; }
        //public int? ProvinceID { get; set; }
        //public int? DistrictID { get; set; }
        //public int? WardID { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
    public class ChangeCustomerInfoWebModel
    {
        public int ID { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
    }
}
