using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;
using MiTienda.Utilities;

namespace MiTienda.Controllers
{
    public class HomeController(
        CategoriaService _categoriaService,
        ProductService _productService
        ): Controller
    {
        public async Task<IActionResult> Index()
        {
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
        public async Task<IActionResult>AddICarrito(int productId, int quantity)
        {

            //obtener el prod q quiero guardar
            var producto = await _productService.GetByIdAsync(productId);

            var carrito = HttpContext.Session.Get<List<CarritoVm>>("Carrito") ?? new List<CarritoVm>();
            if(carrito.Find(x => x.ProductoId == productId)== null)
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
                var updateProduct = carrito.Find(x => x.ProductoId == productId);
                updateProduct!.Cantidad += quantity;
            }

            HttpContext.Session.Set("Carrito", carrito);
            ViewBag.message = "Producto agregado al Carrito";
            return View("ProductDetalle", producto);
        }

        public IActionResult ViewCarrito()
        {
            var carrito = HttpContext.Session.Get<List<CarritoVm>>("Carrito") ?? new List<CarritoVm>();
            return View(carrito);
        }

        public IActionResult removeCarrito(int productId)
        {
            var carrito = HttpContext.Session.Get<List<CarritoVm>>("Carrito");
            var producto = carrito.Find(x => x.ProductoId == productId);
            carrito.Remove(producto!);
            HttpContext.Session.Set("Carrito", carrito);

            return View("ViewCarrito", carrito);
        }

        public IActionResult Privacy()
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
