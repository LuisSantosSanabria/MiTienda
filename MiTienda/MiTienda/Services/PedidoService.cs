using MiTienda.Entidades;
using MiTienda.Models;
using MiTienda.Repositories;

namespace MiTienda.Services
{
    public class PedidoService(OrderRepository _orderRepository)
    {
        public async Task AddAsync(List<CarritoVm> carritoVm,int usuarioId)
        {

            //inf de la orden
            Pedido pedido = new Pedido()
            {
                FechaPedido = DateTime.Now,
                UserId = usuarioId,
                Total = carritoVm.Sum(x => x.Precio * x.Cantidad),
                Articulos = carritoVm.Select(x => new Articulo
                {
                    ProductoId = x.ProductoId,
                    Cantidad = x.Cantidad,
                    Precio = x.Precio
                }).ToList()
            };
            await _orderRepository.AddAsync(pedido);
        }

        public async Task<List<PedidoVM>> GetAllUsuarioAsync(int userId)
        {
            var pedidos = await _orderRepository.GetDetallesAsync(userId);
            var pedidosVM = pedidos.Select(x => new PedidoVM
            {
                PedidoDatos = x.FechaPedido.ToString("dd/MM/yyyy"),
                TotalPedido = x.Total.ToString("C2"),
                Articulos = x.Articulos.Select(x => new ArticulosVm
                {
                    NombreProducto = x.Producto.Nombre,
                    Cantidad = x.Cantidad,
                    Precio = x.Precio.ToString("C2")
                }).ToList()

            }).ToList();

            return pedidosVM;
        }
    }
}
