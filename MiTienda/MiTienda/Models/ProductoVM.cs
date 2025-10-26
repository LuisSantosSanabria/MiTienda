using Microsoft.AspNetCore.Mvc.Rendering;
using MiTienda.Entidades;
using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class ProductoVM
    {
        public int ProdcutoId { get; set; }
        public CategoriaVM Categoria { get; set; }

        // mostrar en combobox
        public List<SelectListItem> Categorias { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Stock { get; set; }
        public string? ImgNombre { get; set; } = null;

        //nos permite obtener una imagen
        public IFormFile? ImageFile { get; set; }
    }
}
