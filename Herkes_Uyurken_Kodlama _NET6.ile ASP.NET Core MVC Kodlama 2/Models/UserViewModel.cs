using System.ComponentModel.DataAnnotations;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Models
{
    public class UserModel
    {

        public Guid Id { get; set; }

        public String Fullname { get; set; }


        public String UserName { get; set; }

        public bool Locked { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Role { get; set; } = "user";

        public string? ProfileImageFileName { get; set; } = "no-image-icon-23485.jpg";
    }

    public class CreateUserModel
    {
        [Required(ErrorMessage = "fullname boş geçilemez")]
        [StringLength(30)]
        public String Fullname { get; set; }

        [Required(ErrorMessage = "isim boş geçilemez")]
        [StringLength(20)]
        public String Username { get; set; }

        public bool Locked { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "user";

        [Required(ErrorMessage = "Şifre boş geçilemez")]
        [MinLength(3, ErrorMessage = "Şifre en az 3 Karakter ollmalı")]
        [MaxLength(8, ErrorMessage = "Şifre en fazla 8 Karakter ollmalı")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre boş geçilemez")]
        [MinLength(3, ErrorMessage = "Şifre en az 3 Karakter ollmalı")]
        [MaxLength(8, ErrorMessage = "Şifre en fazla 8 Karakter ollmalı")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }

        public string? Done { get; set; }
    }

    public class EditUserModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "fullname boş geçilemez")]
        [StringLength(30)]
        public String Fullname { get; set; }

        [Required(ErrorMessage = "isim boş geçilemez")]
        [StringLength(20)]
        public String Username { get; set; }

        public bool Locked { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "user";


        public string? Done { get; set; }


    }

}
