namespace MiTienda.Entidades
{
    public class Articulo
    {
        public int ArticuloId { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        public Pedido? Pedido { get; set; }
        // a que pedido le pertenece
        public Producto? Producto { get; set; }
        // que producto esta dentro de esa orden
    }
}
