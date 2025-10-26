namespace MiTienda.Models
{
    public class CatalogoVM
    {
        public IEnumerable<CategoriaVM> Categorias { get; set; }
        public IEnumerable<ProductoVM> Producto { get; set; }
        public string filterBy { get; set; }
    }
}
