using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;

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

        public async Task<IActionResult> FilterByCategory(int id, string nombre)
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productService.GetCatalogoAsync(categoriaId:id);

            var catalogo = new CatalogoVM { Categorias = categorias, Producto = productos, filterBy=nombre };
            return View("Index", catalogo);
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
