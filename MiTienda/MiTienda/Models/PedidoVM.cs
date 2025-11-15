namespace MiTienda.Models
{
    public class PedidoVM
    {
        public string PedidoDatos { get; set; }
        public string TotalPedido { get; set; }
        public ICollection<ArticulosVm>? Articulos { get; set; }

        public string UsuarioEmail { get; set; }
    }
}
