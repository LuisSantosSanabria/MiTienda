using System.ComponentModel.DataAnnotations;

namespace MiTienda.Entidades
{
    public class Producto
    {
        public int ProdcutoId { get; set; }
        public int CategoriaId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImgNombre { get; set; } = null;

        public Categoria? Categoria { get; set; }
    }
}
