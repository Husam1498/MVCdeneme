using System.ComponentModel.DataAnnotations;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "isim boş geçilemez")]
        [StringLength(20)]
        public String Username { get; set; }

        [Required(ErrorMessage = "Şifre boş geçilemez")]
        [MinLength(3, ErrorMessage = "Şifre en az 3 Karakter ollmalı")]
        [MaxLength(8, ErrorMessage = "Şifre en fazla 8 Karakter ollmalı")]
        public string Password { get; set; }
    }

}
