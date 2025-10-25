using System.ComponentModel.DataAnnotations;

namespace MiTienda.Entidades
{
    public class Categoria
    {
        public string CategoriaId { get; set; }
        [Required]
        public string Nombre { get; set; }

    }
}
