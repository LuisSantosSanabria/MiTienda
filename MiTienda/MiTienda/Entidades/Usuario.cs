using System.ComponentModel.DataAnnotations;

namespace MiTienda.Entidades
{
    public class Usuario
    {
        public int UserId { get; set; }
        [Required]
        public string Nombre{ get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Contrasena { get; set; }
        [Required]
        public string Tipo { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
        // del usuario X dame todas sus pedidos
        // con esto nos facilita
    }
}
