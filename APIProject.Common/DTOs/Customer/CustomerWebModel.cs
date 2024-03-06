

/*----------------------------------------------*
 * Author   : NGuyễn Viết Minh Tiến             *
 * DateTime : 15/12/2021                        *
 * Edit     : Chưa chỉnh sửa                    *
 * Content  : Model trả ra danh sách khách hàng *
 * ---------------------------------------------*/


using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Customer
{
    public class CustomerWebModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public DateTime? DOB { get; set; }
        public string LastLoginDate { get; set; }
        public int? OriginCustomer { get; set; }
        public int EventParticipantCount { get; set; }
    }
   
}
