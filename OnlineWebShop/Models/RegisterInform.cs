using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineWebShop.Models
{
    public class RegisterInform
    {
        [StringLength(250)]
        [Required(ErrorMessage ="Thông tin không được để trống")]
        [Display(Name ="Họ và tên")]
        public string _name { get; set; }

        [Required(ErrorMessage ="Thông tin không được để trống")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Vui lòng nhật đúng địa chỉ mail")]
        [Display(Name = "Email")]
        public string _email { get; set; }

        [Required(ErrorMessage = "Thông tin không được để trống")]
        [Display(Name = "Địa chỉ")]
        public string _address { get; set; }

        [Required(ErrorMessage = "Thông tin không được để trống")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Vui lòng nhập đúng số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public int _phone { get; set; }

        [Required(ErrorMessage = "Thông tin không được để trống")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.DateTime)]
        public DateTime _birthday { get; set; }

        [Required(ErrorMessage = "Thông tin không được để trống")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string _pass { get; set; }

        [Required(ErrorMessage = "Thông tin không được để trống")]
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("_pass",ErrorMessage ="Xác nhận mật khẩu không trùng khớp.")]
        public string _passconfirm { get; set; }
    }
}