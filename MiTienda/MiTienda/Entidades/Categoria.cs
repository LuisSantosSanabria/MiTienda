using System.ComponentModel.DataAnnotations;

namespace MiTienda.Entidades
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        [Required]
        public string Nombre { get; set; }

        public ICollection<Producto> Productos { get; set; }

    }
}
