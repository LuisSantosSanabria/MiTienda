using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Clave { get; set; }
    }
}
