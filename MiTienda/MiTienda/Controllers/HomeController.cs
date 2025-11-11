using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;
using MiTienda.Utilities;

namespace MiTienda.Controllers
{
    public class HomeController(
        CategoriaService _categoriaService,
        ProductService _productService,
        PedidoService _pedidoService
        ): Controller
    {
        public async Task<IActionResult> Index()
        {
            // si es adm
            if (User.Identity!.IsAuthenticated && User.IsInRole("Adm"))
            {
                return RedirectToAction("Index", "Admin");
            }

            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productService.GetCatalogoAsync();
            var catalogo = new CatalogoVM { Categorias = categorias, Producto = productos };
            return View(catalogo);
        }

        //filtrar categoria
        public async Task<IActionResult> FilterByCategory(int id, string nombre)
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productService.GetCatalogoAsync(categoriaId:id);

            var catalogo = new CatalogoVM { Categorias = categorias, Producto = productos, filterBy=nombre };
            return View("Index", catalogo);
        }

        [HttpPost]
        public async Task<IActionResult> FilterByBuscar(string value)
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productService.GetCatalogoAsync(buscar: value);

            var catalogo = new CatalogoVM { Categorias = categorias, Producto = productos, filterBy = $"Resultados para:{value}" };
            return View("Index", catalogo);
        }

        //detalles del producto
        public async Task<IActionResult> ProductDetalle(int id)
        {
            var producto = await _productService.GetByIdAsync(id);
            return View(producto);
        }

        //agregar carrito
        [HttpPost]
        public async Task<IActionResult> AddICarrito(int productId, int quantity)
        {
            // Verificar si el usuario está autenticado
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["LoginMessage"] = "Debes iniciar sesión para agregar productos al carrito.";
                return RedirectToAction("Login", "Registro");
            }

            // Obtener el producto desde la base de datos
            var producto = await _productService.GetByIdAsync(productId);
            if (producto == null)
            {
                TempData["message"] = "El producto no existe o fue eliminado.";
                return RedirectToAction("Index");
            }

            // Crear clave de sesión única por usuario
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionKey = $"Carrito_{userId}";

            // Obtener el carrito actual o crear uno nuevo
            var carrito = HttpContext.Session.Get<List<CarritoVm>>(sessionKey) ?? new List<CarritoVm>();

            // Buscar si el producto ya está en el carrito
            var existingProduct = carrito.FirstOrDefault(x => x.ProductoId == productId);
            if (existingProduct == null)
            {
                carrito.Add(new CarritoVm
                {
                    ProductoId = productId,
                    ImgNombre = producto.ImgNombre,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = quantity
                });
            }
            else
            {
                existingProduct.Cantidad += quantity;
            }

            // Guardar el carrito actualizado en la sesión
            HttpContext.Session.Set(sessionKey, carrito);

            // Guardar un mensaje temporal para mostrar después del redirect
            TempData["message"] = "Producto agregado al carrito correctamente.";

            // Redirigir al detalle del producto (esto recarga el layout y el contador)
            return RedirectToAction("ProductDetalle", new { id = productId });
        }



        public IActionResult ViewCarrito()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Registro");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionKey = $"Carrito_{userId}";

            var carrito = HttpContext.Session.Get<List<CarritoVm>>(sessionKey) ?? new List<CarritoVm>();
            return View(carrito);
        }

        //public IActionResult removeCarrito(int productId)
        //{
        //    if (!User.Identity!.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Registro");
        //    }

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var sessionKey = $"Carrito_{userId}";

        //    var carrito = HttpContext.Session.Get<List<CarritoVm>>(sessionKey);
        //    var producto = carrito?.FirstOrDefault(x => x.ProductoId == productId);

        //    if (producto != null)
        //    {
        //        carrito.Remove(producto);
        //        HttpContext.Session.Set(sessionKey, carrito);
        //    }

        //    return View("ViewCarrito", carrito);
        //}

        public IActionResult removeCarrito(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionKey = $"Carrito_{userId}";

            var carrito = HttpContext.Session.Get<List<CarritoVm>>(sessionKey) ?? new List<CarritoVm>();

            var producto = carrito.FirstOrDefault(x => x.ProductoId == productId);
            if (producto != null)
            {
                carrito.Remove(producto);
                HttpContext.Session.Set(sessionKey, carrito);
            }

            return View("ViewCarrito", carrito);
        }


        //[HttpPost]
        //public async Task<IActionResult> Pagar()
        //{
        //    if (!User.Identity!.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Registro");
        //    }

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var sessionKey = $"Carrito_{userId}";

        //    var carrito = HttpContext.Session.Get<List<CarritoVm>>(sessionKey);

        //    if (carrito == null || carrito.Count == 0)
        //    {
        //        TempData["ErrorMessage"] = "Tu carrito está vacío.";
        //        return RedirectToAction("ViewCarrito");
        //    }

        //    // Registrar el pedido en la base de datos
        //    await _pedidoService.AddAsync(carrito, int.Parse(userId));

        //    // Vaciar carrito después de la compra
        //    HttpContext.Session.Remove(sessionKey);

        //    return View("VentaCompletada");
        //}

        [HttpPost]
        public async Task<IActionResult> Pagar()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionKey = $"Carrito_{userId}";

            var carrito = HttpContext.Session.Get<List<CarritoVm>>(sessionKey) ?? new List<CarritoVm>();

            if (carrito.Count == 0)
            {
                TempData["ErrorMessage"] = "Tu carrito está vacío.";
                return RedirectToAction("ViewCarrito");
            }

            await _pedidoService.AddAsync(carrito, int.Parse(userId));

            HttpContext.Session.Remove(sessionKey);

            return View("VentaCompletada");
        }


        public IActionResult VentaCompletada()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
