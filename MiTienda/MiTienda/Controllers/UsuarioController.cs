using Microsoft.AspNetCore.Mvc;
using MiTienda.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MiTienda.Controllers
{
    [Authorize]
    public class UsuarioController (PedidoService _pedidoService) : Controller
    {
        public async Task<IActionResult> MiPedido()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var pedidosvm = await _pedidoService.GetAllUsuarioAsync(int.Parse(userId));

            return View(pedidosvm);
        }
    }
}
