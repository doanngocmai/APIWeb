using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Utils
{
    public class SystemParam
    {
        #region -- CONSTANT --
        // Notification
        public const string NOTI_TITLE = "Savico Megamall thông báo";
        public const string APP_ID = "a067a1ac-0557-4ef2-af8a-bcba74165857";
        public const string Authorization = "Basic :YzVmZDc3OTctZTdmMS00MTM1LTllOTgtMjQzYTRkZjIwMzlj";
        public const string ANDROID_CHANNEL_ID = "435adbbf-98a4-447c-9c94-39de9ffec195";
        public const string URL_ONESIGNAL = "https://onesignal.com/api/v1/notifications";
        public const string URL_BASE_https = "Basic ://onesignal.com/api/v1/notifications";

        public const int NOTI_READ = 1;
        public const int NOTI_NOT_READ = 0;

        public const int NOTI_ADMIN = 1;
        public const int NOTI_CUSTOMER = 0;


        // Upload File
        public const string FILE_NAME = "Images";
        public const string FILE = "file";
        // Config
        public const string LINK_SURVERY = "LinkSurvery";

        public const string LINK_HOTLINE = "LinkHotline";
        public const string LINK_WEBSITE = "LinkWebsite";
        public const string LINK_FACEBOOK = "LinkFacebook";

        public const string POINT_ADD = "PoinAdd";
        public const string ORDER_VALUE = "OrderValue";

        //OTP
        public const string SMS_MESSAGE = "Savico Mall: Ma OTP dang nhap Savico Megamall của Quy khach la:{0}.Khong cung cap ma nay cho bat ky ai. LH 0246.266.8855";
        public const string SMS_CLIENT_ID = "efa66179-1eb9-4187-9c0f-52fc99388492";
        public const string SMS_USERNAME = "savico@sms.sami.vn";
        public const string SMS_PASSWORD = "Savico@321";
        public const string SMS_PRIVATE_KEY_PATH = @".\PrivateKey\id_rsa";
        public const string SMS_TOKEN_URL = "https://auth.sami.vn:8443/api/authenticate/token";
        public const string SMS_URL = @"https://sms.sami.vn:8558/api/sms/send";
        public const int SMS_COOPERATE_ID = 3;
        public const string SMS_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c24iOiJwYW5kYWF1dG8iLCJzaWQiOiI5ZjhhNDg5MC03MWIwLTQ3OTYtODliZS00ZWVlNWFhMzE0OTIiLCJvYnQiOiIiLCJvYmoiOiIiLCJuYmYiOjE1OTQwMjgyOTksImV4cCI6MTU5NDAzMTg5OSwiaWF0IjoxNTk0MDI4Mjk5fQ.rq2Lel0HAwbc2d5PdFvamdTwWuVOMD9-LXb0ccsy6Ec";
        public const string SMS_TRAFFIC_MESSAGE = "THONG BAO PHAT NGUOI: Oto BKS {0} da vi pham giao thong. Truy cap ung dung G-CHECK tren MH Gotech de tra cuu thong tin. Chuc quy khach lai xe an toan!";
        public const string SMS_SUCCESS_CODE = "000";
        public const string SMS_OTP_FAIL = "-15";
        public const string SMS_OTP_EXPIRE = "-14";
        public const int MAX_TIME_OTP = 5; // Số lần gửi OTP tối đa
        public const int TIME_EXPIRE_OTP = 15; // Thời gian OTP hết hạn (Minute)
        public const int TIME_OTP_RESET = 5; // Thời gian OTP gửi lại được reset (Minute)


        public const int PAGE_DEFAULT = 1;
        public const int LIMIT_DEFAULT = 10;
        public const int LIMIT_BANNER = 4;
        public const int LIMIT_HOT_NEWS = 3;
        public const int LIMIT_MAX_DEFAULT = 100;

        public const int ACTIVE = 1;
        public const int ACTIVE_FALSE = 0;

        public const int SORT_ASCENDING = 1;
        public const int SORT_DESCENDING = 2;

        public const string CONVERT_DATETIME = "dd/MM/yyyy";
        public const string CONVERT_DATETIME_HAVE_HOUR = "HH:mm dd/MM/yyyy";

        // OTP
        public const int OTP_MAX_QUANTITY = 5;

        // Token Type
        public const string TOKEN_TYPE_CUSTOMER = "1";
        public const string TOKEN_TYPE_USER = "2";

        // Customer
        public const int CUSTOMER_ORIGIN_REGISTER = 1; // Sử dụng App
        public const int CUSTOMER_ORIGIN_PG = 2; // PG tạo


        public const int NEWS_TYPEPOST_POSTED = 2; //  Đăng bài  
        public const int NEWS_TYPEPOST_DRAFT = 1;  //  Lưu nháp

        // Price
        public const long MILLION = 1000000;
        // News
        public const int TYPE_NEWS_PROMOTION = 1;//Loại khuyến mại 
        public const int TYPE_NEWS_EVENT = 2;//loại sự kiện
        public const int TYPE_NEWS_RECRUIT = 3;//loại tuyển dụng
        public const int TYPE_NEWS_NEWS_EVENT = 4;//loại tin tức sự kiện
        public const int TYPE_NEWS_UTILITIES = 5;//loại tiện ích

        //Gift
        public const int TYPE_GIFT_PRESENTS = 1;//loại quà tặng
        public const int TYPE_GIFT_VOURCHER = 2;//loại vourcher

        //GiftCode
        public const int STATUS_GIFTCODE_NOT_EXCHANGE = 0;//Chưa đổi
        public const int STATUS_GIFTCODE_EXCHANGE = 1;//Đã đổi
        public const int STATUS_GIFTCODE_USED = 2;//Đã sử dụng
        // District
        public const int DISTRIC_ID_LB = 4;// id của quận Long Biên       
        public const string DISTRIC_LB = "Long Biên";
        public const string DISTRIC_GL = "Gia Lâm";
        public const int DISTRIC_ID_GL = 18;// id của quận gia lâm
        //Provice
        public const string PROVINCE_BN = "Bắc Ninh";
        public const int PROVINCE_ID_BN = 27;// id của Tỉnh bắc  NINH
        public const string PROVINCE_HY = "Hưng Yên";
        public const int PROVINCE_ID_HY = 33;// id của tỉnh Hưng Yên
        public const string OTHER = "Khác";
        //Gender
        public const string Gender_Boy = "Nam";
        public const int IDBoy = 0;
        public const string Gender_Girl = "Nữ";
        public const int IDGirl = 1;
        // Status
        public const int Status_Activate = 1;
        // Age
        public const int Age_15_22 = 1;
        public const string Age_15_22_STR = "15-22";
        public const int Age_23_35 = 2;
        public const string Age_23_35_STR = "23-35";
        public const int Age_36_45 = 3;
        public const string Age_36_45_STR = "36-45";
        public const int Age_Above_45 = 4;
        public const string Age_Above_45_STR = "trên 45";

        // Same Filter Type
        public const int TYPE_SAME_FILTER_WEEK = 1;
        public const int TYPE_SAME_FILTER_MONTH = 2;
        public const int TYPE_SAME_FILTER_QUARTER = 3;
        public const int TYPE_SAME_FILTER_YEAR = 4;


        #endregion
        #region -- API RESPONSE --

        // Default
        public const int SUCCESS = 1;
        public const int ERROR = 0;
        public const int SUCCESS_CODE = 200;
        public const string MESSAGE_SUCCESS = "Thành công";
        public const int ERROR_CODE = 501;
        public const string MESSAGE_ERROR = "Thất bại";
        public const string SERVER_ERROR = "Hệ thống đang bảo trì";
        public const int TOKEN_INVALID = 401;
        public const string MESSAGE_TOKEN_INVALID = "Đăng nhập để thực hiện chức năng này";
        public const int PERMISSION_INVALID = 402;
        public const string MESSAGE_PERMISSION_INVALID = "Bạn không có quyền thực hiện chức năng này";
        public const int TOKEN_ERROR = 403;
        public const string MESSAGE_TOKEN_ERROR = "Tài khoản của bạn đã đăng nhập ở nơi khác";

        // Login
        public const int ERROR_LOGIN_FIELDS_INVALID = 1;
        public const string MESSAGE_LOGIN_FIELDS_INVALID = "Vui lòng nhập số điện thoại";
        public const int ERROR_LOGIN_FAIL = 2;
        public const string MESSAGE_LOGIN_FAIL = "Số điện thoại không tồn tại";
        public const int ERROR_LOGIN_ACCOUNT_LOCK = 3;
        public const string MESSAGE_LOGIN_ACCOUNT_LOCK = "Tài khoản đã bị khóa";
        public const int ERROR_LOGIN_CHANGE_PASS = 4;
        public const string MESSAGE_LOGIN_CHANGE_PASS = "Bạn đã thay đổi mật khẩu vui lòng nhập mật khẩu mới!";
        public const int ERROR_LOGIN_FAIL_PASS = 5;
        public const string MESSAGE_LOGIN_FAIL_PASS = "Sai mật khẩu vui lòng nhập lại mật khẩu";

        public const int ERROR_PHONE_NOT_REGISTER = 1;
        public const string MESSAGE_PHONE_NOT_REGISTER = "Số điện thoại chưa được đăng ký";

        // Register
        public const int ERROR_REGISTER_FIELDS_INVALID = 1;
        public const string MESSAGE_REGISTER_FIELDS_INVALID = "Vui lòng nhập đầy đủ thông tin bắt buộc";
        public const int ERROR_REGISTER_PHONE_INVALID = 2;
        public const string MESSAGE_REGISTER_PHONE_INVALID = "Số điện thoại không đúng định dạng";
        public const int ERROR_REGISTER_PHONE_EXIST = 3;
        public const string MESSAGE_REGISTER_PHONE_EXIST = "Số điện thoại đã tồn tại";
        public const int ERROR_REGISTER_EMAIL_INVALID = 4;
        public const string MESSAGE_REGISTER_EMAIL_INVALID = "Email không đúng định dạng";
        public const int ERROR_REGISTER_EMAIL_EXIST = 5;
        public const string MESSAGE_REGISTER_EMAIL_EXIST = "Email đã tồn tại";
        public const int ERROR_OTP_TRY_EXCEED = 6;
        public const string MESSAGE_OTP_TRY_EXCEED = "Bạn đã vượt quá số lần gửi lại mã OTP , xin vui lòng thử lại sau 5 phút";
        public const int ERROR_PHONE_NOT_EXIST = 7;
        public const string MESSAGE_PHONE_NOT_EXIST = "Số điện thoại không tồn tại";
        public const int ERROR_PHONE_NOT_OTP = 8;
        public const string MESSAGE_PHONE_NOT_OTP = "Mã OTP không hợp lệ";
        public const int ERROR_PHONE_OTP_EXPIRED = 9;
        public const string MESSAGE_PHONE_OTP_EXPIRED = "Mã OTP đã hết hạn";
        // Receive Address 
        public const int ERROR_RECEIVE_ADDRESS_NOT_EXIST = 1;
        public const string MESSAGE_RECEIVE_ADDRESS_NOT_EXIST = "Địa chỉ nhận hàng không tồn tại";

        // Upload File
        public const int ERROR_FILE_NOT_FOUND = 1;
        public const string MESSAGE_FILE_NOT_FOUND = "Không tìm thấy ảnh tải lên ";

        //Product Item : 
        public const int ERROR_PRODUCTITEM_NOT_FOUND = 1;
        public const string MESSAGE_PRODUCTITEM_NOT_FOUND = "Sản phẩm không tồn tại";

        // Notifcation
        public const int ERROR_NOTIFICATION_NOT_FOUND = 1;
        public const string MESSAGE_NOTIFICATION_NOT_FOUND = "Thông báo không tồn tại";
        // Customer 
        public const int ERROR_NOT_FOUND_CUSTOMER = 604;
        public const string MESSAGE_NOT_FOUND_CUSTOMER = "Không tìm thấy khách hàng";
        public const string MESSAGE_LOCK_ACOUNT = "Khách hàng đang bị khóa tài khoản";
        public const int ERROR_LOCK_ACOUNT = -1;
        public const int NOT_FOUND_CUSTOMER = -2;
        public const int ROLE_CUSTOMER = 1;
        public const int ROLE_STAFF = 2;
        public const int ERROR_REQUIRED_COUSTOMER = -3;
        public const string MESSAGE_REQUIRED_COUSTOMER = "Yêu cầu nhập đủ thông tin";
        //Staff
        public const int ERROR_PHONE_NOT_FOUND = -1;
        public const string MESSAGE_CHECK_PHONE_NOT_FOUND = "Số điện thoại không tồn tại";

        public const int ERROR_PHONE_NOT_VALID = -2;
        public const string MESSAGE_PHONE_NOT_VALID = "Số điện thoại không đúng định dạng";
        public const int ERROR_STAFF_FIELDS_INVALID = -2;
        public const string MESSAGE_STAFF_FIELDS_INVALID = "Vui lòng nhập đầy đủ trường bắt buộc";
        public const string MESSAGE_NOT_FOUND_STAFF = "Không tìm thấy nhân viên";
        public const string MESSAGE_LOCK_STAFF = "Tài khoản đang bị khóa ";
        public const int ERROR_NOT_FOUND_STAFF = 604;
        // Password :
        public const int ERROR_CODE_CUSOTMER_LOCK = 1;
        public const string MESSAGE_LOCK_CUSTOMER = "Tài khoản khách hàng đang bị khóa";
        public const string MESSAGE_NOT_CONFIRM_OTP = "Vui Lòng xác nhận OTP trước khi đổi MK";
        public const int CODE_NOT_CONFIRM_OTP = 2;
        public const int ERROR_CUSOTMER_NOT_EXSIST = 1;
        public const string MESSAGE_CUSOTMER_NOT_EXSIST = "Khách hàng không tồn tại";
        public const int ERROR_CHECK_PASSWORD_NOT_EXSIST = 3;
        public const string MESSAGE_CHECK_PASSWORD_NOT_EXSIST = "Mật khẩu không đúng";
        // OTP 
        public const int ERROR_CODE_INVALID_OTP = -1;
        public const string MESSAGE_CODE_INVALID_OTP = "Mã OTP Không hợp lệ";
        public const int ERROR_CODE_UNCONFIRMED = -2;
        public const string MESSAGE_UNCONFIRMED = "Bạn chưa xác nhận OTP";
        public const string MESSAGE_PHONE_NOT_FOUND = "Số điện thoại chưa được đăng kí";
        public const int ERROR_CODE_PHONE_NOT_FOUND = -3;
        public const int ERROR_OTP_MAX_QUANTITY_EXCEED = -4;
        public const string MESSAGE_OTP_MAX_QUANTITY_EXCEED = "Bạn đã vượt quá số lần gửi mã OTP .Vui lòng thử lại sau 5 phút ";

        //Category
        public const int ERROR_CATEGORY_FIELDS_INVALID = 1;
        public const string MESSAGE_CATEGORY_FIELDS_INVALID = "Vui lòng nhập đầy đủ trường bắt buộc";
        public const int ERROR_CATEGORY_NOT_FOUND = 2;
        public const string MESSAGE_CATEGORY_NOT_FOUND = "Danh mục không tồn tại";
        public const int ERROR_CATEGORY_EXIST_STALL = 3;
        public const string MESSAGE_CATEGORY_EXIST_STALL = "Không thể xóa ngành hàng khi vẫn tồn tại gian hàng thuộc ngành hàng";

        // User
        public const int ERROR_USER_NOT_FOUND = 1;
        public const string MESSAGE_USER_NOT_FOUND = "Tài khoản không tồn tại";

        public const int ERROR_CHANGE_PASSWORD_WRONG = -1;
        public const string MESSAGE_CHANGE_PASSWORD_WRONG = "Mật khẩu cũ không đúng";

        // Role
        public const int ERROR_ROLE_NOT_FOUND = 1;
        public const string MESSAGE_ROLE_NOT_FOUND = "Phân quyền không tồn tại";
        public const int ERROR_ROLE_NAME_ALREADY_EXIST = 2;
        public const string MESSAGE_ROLE_NAME_ALREADY_EXIST = "Tên phân quyền đã tồn tại";
        public const int ERROR_ROLE_USER_STILL_EXIST = 3;
        public const string MESSAGE_ROLE_USER_STILL_EXIST = "Không thể xóa phân quyền khi vẫn tồn tại tài khoản thuộc phân quyền";

        //Stall
        public const int TYPE_TOP_STALL_BILL_MOST = 1;
        public const int TYPE_TOP_STALL_BILL_LEAST = 2;
        public const int ERROR_STALL_FIELDS_INVALID = 1;
        public const string MESSAGE_STALL_FIELDS_INVALID = "Vui lòng nhập đầy đủ trường bắt buộc";
        public const int ERROR_STALL_NOT_FOUND = 2;
        public const string MESSAGE_STALL_NOT_FOUND = "Gian hàng không tồn tại";
        public const int ERROR_STALL_NOT_PHONE = 3;
        public const string MESSAGE_STALL_NOT_PHONE = "Mã gian hàng đã tồn tại";
        public const int ERROR_STALL_EXIST_NEWS = 4;
        public const string MESSAGE_STALL_EXIST_NEWS = "Không thể xóa gian hàng khi vẫn tồn tại bài viết liên quan gian hàng";
        //News
        public const int ERROR_CODE_NOT_FOUND_NEWS = -1;
        public const string MESSAGE_CODE_NOT_FOUND_NEWS = "Bài viết không tồn tại";
        public const string MESSAGE_NEWS_TYPE_NOT_AVAILABLE = "Loại tin tức không khả dụng";
        public const string NEWS_CODE_DEFAULT = "TT";
        public const int ERROR_NEWS_FIELDS_INVALID = 1;
        public const string MESSAGE_NEWS_FIELDS_INVALID = "Vui lòng nhập đầy đủ trường bắt buộc";
        public const int ERROR_NEWS_NOT_FOUND = 2;
        public const string MESSAGE_NEWS_NOT_FOUND = "Bài viết không tồn tại";
        public const int ERROR_NEWS_NOT_PHONE = 3;
        public const string MESSAGE_NEWS_NOT_PHONE = "Số điện thoại đại lý đã tồn tại";
        public const int ERROR_QUANTY_EVENT_GIFT_INVALID = 4;
        public const string MESSAGE_QUANTY_EVENT_GIFT_INVALID = "Số lượng quà tặng phải lớn hơn 0";
        public const int ERROR_QUANTY_GIFT_MAX = 5;
        public const string MESSAGE_QUANTY_GIFT_MAX = "Số lượng quà tặng phải nhỏ hơn hoặc bằng số lượng quà tặng tồn kho";
        public const int ERROR_QUANTY_GIFT_INITIAL = 6;
        public const string MESSAGE_QUANTY_GIFT_INITIAL = "Số lượng quà tặng phải lớn hơn hoặc bằng số lượng ban đầu";
        public const int ERROR_NEWS_EXIST_STALL = 4;
        public const string MESSAGE_NEWS_EXIST_STALL = "Không thể xóa bài viết khi vẫn tồn tại gian hàng liên quan bài viết";
        public const int ERROR_MISSING_DATE = -2;
        public const string MESSAGE_MISSING_DATE = "Sự kiện chưa kích hoạt";

        //Config
        public const int ERROR_CONFIG_FIELDS_INVALID = 1;
        public const string MESSAGE_CONFIG_FIELDS_INVALID = "Vui lòng nhập đầy đủ trường bắt buộc";
        public const int ERROR_CONFIG_NOT_FOUND = 2;
        public const string MESSAGE_CONFIG_NOT_FOUND = "Cấu hình không tồn tại";
        //Point History
        public const int TYPE_MEMBER_HISTORY_EARN_POINT = 1;//Lịch sử tích điểm
        public const int TYPE_MEMBER_HISTORY_EXCHANGED_POINT = 2;//lịch sử đổi điểm

        //Notification
        public const int TYPE_NOTIFICATION_EARN_POINT = 1; // Thông báo tích điểm
        public const int TYPE_NOTIFICATION_NEWS = 2; // Thông báo bài viết
        //Survery
        public const int ERROR_SURVERY_NOT_FOUND = 2;
        public const string MESSAGE_SURVERY_NOT_FOUND = "Câu hỏi khảo sát không tồn tại";
        public const int ERROR_SURVEY_QUESTION_NOT_FOUND = 3;
        public const string MESSAGE_SURVEY_QUESTION_NOT_FOUND = "Câu hỏi khảo sát không tồn tại";
        public const int ERROR_SURVEY_ANSWER_NOT_FOUND = 4;
        public const string MESSAGE_SURVEY_ANSWER_NOT_FOUND = "Câu trả lời khảo sát không tồn tại";
        //Gift
        public const int ERROR_GIFT_FIELDS_INVALID = 1;
        public const string MESSAGE_GIFT_FIELDS_INVALID = "Vui lòng nhập đầy đủ trường bắt buộc";
        public const int ERROR_GIFT_NOT_FOUND = 2;
        public const string MESSAGE_GIFT_NOT_FOUND = "Quà tặng/Vourcher không tồn tại";
        public const int ERROR_GIFT_NUMBER_NEGATIVE = 3;
        public const string MESSAGE_GIFT_NUMBER_NEGATIVE = "Điểm và số lượng nhỏ nhất bằng 1";
        public const int ERROR_GIFT_CODE_DUPLICATE = 4;
        public const string MESSAGE_GIFT_CODE_DUPLICATE = "Mã voucher/quà tặng đã tồn tại";
        public const string MESSAGE_GIFT_CODE_NOT_FOUND = "Mã quà tặng/Vourcher không tồn tại";
        public const int ERROR_VOUCHER_NOT_FOUND = 5;
        public const string MESSAGE_VOUCHER_NOT_FOUND = "Vourcher không tồn tại";
        public const int ERROR_EXCHANGED_VOUCHER_NOT_ENOUGH_POINT = 6;
        public const string MESSAGE_EXCHANGED_VOUCHER_NOT_ENOUGH_POINT = "Bạn không đủ điểm để đổi Voucher";
        public const int ERROR_EXCHANGED_VOUCHER_EMPTY= 6;
        public const string MESSAGE_EXCHANGED_VOUCHER_EMPTY = "Số lượng Voucher đã hết";
        public const int ERROR_USED_VOUCHER = 7;
        public const string MESSAGE_USED_VOUCHER = "Voucher đã được sử dụng";

        public const int ERROR_VOUCHER_POINT_NEGATIVE = 8;
        public const string MESSAGE_VOUCHER_POINT_NEGATIVE = "Vui lòng nhập đầy đủ trường bắt buộc";
        public const int ERROR_ALREADY_EXCHANGED_VOUCHER_FREE = 9;
        public const string MESSAGE_ALREADY_EXCHANGED_VOUCHER_FREE = "Bạn đã lấy mã voucher này";

        public const int ERROR_GIFT_EXIST_EVENT = 10;
        public const string MESSAGE_GIFT_EXIST_EVENT = "Quà tặng đã tồn tại trong chiến dịch";

        //Inport Excel Gift
        public const string IMPORT_SAMPLE_APARTMENT = "Mau_import.xlsx";
        public const string IMPORT_SAMPLE_CUSTOMER = "Mau_import_KH.xlsx";
        public const string EXPORT_SAMPLE_CUSTOMER_EXPORT = "Danh_sach_KH.xlsx";
        public const string EXPORT_SAMPLE_ZALO_OA_NEW = "Danh_sach_KH";
        public const string EXPORT_SAMPLE_CUSTOMER_GIFT_EXCHANGE = "Danh_sach_KHDQ.xlsx";
        public const string EXPORT_SAMPLE_CUSTOMER_GIFT_EXCHANGE_NEW = "Danh_sach_KHDQ";
        public const string EXPORT_SAMPLE_SUM_BILL_CATEGORY = "Sum_bill_category.xlsx";
        public const string EXPORT_SAMPLE_SUM_BILL_CATEGORY_NEW = "Sum_bill_category";
        public const string EXPORT_SAMPLE_SUM_BILL_STALL = "Sum_bill_Stall.xlsx";
        public const string EXPORT_SAMPLE_SUM_BILL_STALL_NEW = "Sum_bill_Stall";
        public const string EXPORT_SAMPLE_GH_TGTHDDQ = "Bieu_do_GH_TGTHDDQ.xlsx";
        public const string EXPORT_SAMPLE_GH_TGTHDDQ_NEW = "Bieu_do_GH_TGTHDDQ";
        public const string EXPORT_SAMPLE_GH_TSHDDQTTT = "Bieu_do_GH_TSHDDQTTT.xlsx";
        public const string EXPORT_SAMPLE_GH_TSHDDQTTT_NEW = "Bieu_do_GH_TSHDDQTTT";
        public const string VOUCHER_USED_NOT_STR = "CHƯA SỬ DỤNG";
        public const int VOUCHER_STATUS_NOT_USED = 0; // chưa sử dụng
        public const string VOUCHER_USED_STR = "ĐÃ SỬ DỤNG"; 
        public const int VOUCHER_STATUS_USED = 1; // Đã sử dụng

        public const int ERROR_IMPORT_EXCEL_VOUCHER_ERROR = 3;
        public const string MESSAGE_IMPORT_EXCEL_VOUCHER_ERROR = "Import Excel voucher thất bại";

        // Import Excel Customer
        public const int ERROR_IMPORT_EXCEL_CUSTOMER_ERROR = 1;
        public const string MESSAGE_IMPORT_EXCEL_CUSTOMER_ERROR = "Import Excel khách hàng thất bại";

        //UsageFrequency
        public const int ERROR_USAGE_FREQUENCY_NOT_FOUND = -1;
        public const string MESSAGE_USAGE_FREQUENCY_NOT_FOUND = "Không có lịch sử dụng App";

        //EventParticipant
        public const int ERROR_CREATE_QRCODE_INVALID = 1;
        public const string MESSAGE_CREATE_QRCODE_INVALID = "Vui lòng nhập đầy đủ trường bắt buộc";

        public const int ERROR_SCAN_QRCODE_INVALID = 1;
        public const string MESSAGE_SCAN_QRCODE_INVALID = "Mã QR không hợp lệ";
        public const int ERROR_EVENT_INVALID = 2;
        public const string MESSAGE_EVENT_INVALID = "Sự kiện không hợp lệ";
        public const int ERROR_EVENT_GIFT_EMPTY = 3;
        public const string MESSAGE_EVENT_GIFT_EMPTY = "Số lượng quà tặng đã hết";
        public const int ERROR_EVENT_ALREADY_PARTICIPATE_TODAY = 4;
        public const string MESSAGE_EVENT_ALREADY_PARTICIPATE_TODAY = "Khách hàng đã tham gia sự kiện trong ngày hôm nay";
        public const int ERROR_EVENT_STALL_EMPTY = 5;
        public const string MESSAGE_EVENT_STALL_EMPTY = "Vui lòng nhập gian hàng ";
        public const int ERROR_EVENT_PRICE_EMPTY = 6;
        public const string MESSAGE_EVENT_PRICE_EMPTY = "Vui lòng nhập tổng tiền hóa đơn";
        public const int ERROR_EVENT_IMAGE_EMPTY = 7;
        public const string MESSAGE_EVENT_IMAGE_EMPTY = "Vui lòng nhập hình ảnh hóa đơn";
        public const int ERROR_EVENT_CODE_EMPTY = 8;
        public const string MESSAGE_EVENT_CODE_EMPTY = "Vui lòng nhập số hóa đơn";


        #endregion
    }
}
