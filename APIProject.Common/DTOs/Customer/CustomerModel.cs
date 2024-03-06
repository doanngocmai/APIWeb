/*-----------------------------------
 * Author   : NGuyễn Viết Minh Tiến
 * DateTime : 27/11/2021
 * Edit     : - THêm mật khẩu lần 2
 * Content  : Model Customer 
 * ----------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models
{
    public class CustomerModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DeviceID { get; set; }
        public int Role { get; set; }
        public int Point { get; set; }
        public string Token { get; set; }
    }

    public class CreateCustomer
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DeviceID { get; set; }
        public int Role { get; set; }
    }
    
    public class CustomerDetailModel
    {
        public string IdentityNumber { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string CustomerCode { get; set; }
        public int? WardID { get; set; }
        public string WardName { get; set; }
        public int? DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int? ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string Job { get; set; }
        public long Point { get; set; }
    }
}
