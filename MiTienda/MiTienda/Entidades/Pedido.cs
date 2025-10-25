namespace MiTienda.Entidades
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public DateTime FechaPedido { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }

        public Usuario? Usuario { get; set; } = null;
        // de esta orden obtner el usuario
        public ICollection<Articulo> Articulos { get; set; }
        // de esta pedido quiero saber todos los articulso que se piden
    }
}
