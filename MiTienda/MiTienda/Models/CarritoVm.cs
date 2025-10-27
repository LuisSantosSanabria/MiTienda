namespace MiTienda.Models
{
    public class CarritoVm
    {
        public int ProductoId { get; set; }
        public string ImgNombre { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
