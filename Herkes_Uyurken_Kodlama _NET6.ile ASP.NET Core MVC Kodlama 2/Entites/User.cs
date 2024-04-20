using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Entites
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(30)]
        public String Fullname { get; set; }

        [Required]
        [StringLength(50)]
        public String UserName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(8)]
        public String Password { get; set; }


        public bool Locked { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "user";

        [StringLength(255)]
        public string? ProfileImageFileName { get; set; } = "no-image-icon-23485.jpg";




    }

}
