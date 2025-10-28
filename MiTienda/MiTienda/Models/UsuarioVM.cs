using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class UsuarioVM
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Clave { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public string RepetirClave { get; set; }
    }
}
