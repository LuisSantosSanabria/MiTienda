using System.ComponentModel.DataAnnotations;

namespace MiTienda.Entidades
{
    public class Usuario
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Nombre{ get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Contrasena { get; set; }
        [Required]
        public string Tipo { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
        // del usuario X dame todas sus pedidos
        //herramienta de EF Core para navegar desde un objeto Usuario hasta todos sus objetos Pedido asociados
    }
}
