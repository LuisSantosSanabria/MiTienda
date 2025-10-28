using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiTienda.Models;
using MiTienda.Services;

namespace MiTienda.Controllers
{
    [Authorize(Roles = "Adm")]
    public class ProductoController(ProductService _productService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            //obtenr toda la lista de categorias
            var productos = await _productService.GetAllAsync();
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var productoVM = await _productService.GetByIdAsync(id);
            return View(productoVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductoVM entityVm)
        {

            //validar el estado
            ViewBag.message = null;
            ModelState.Remove("Categorias");
            ModelState.Remove("Categoria.Nombre");
            if (!ModelState.IsValid) return View(entityVm);


            if (entityVm.ProdcutoId == 0)
            {
                await _productService.AddAsync(entityVm);
                ModelState.Clear();
                entityVm = new ProductoVM();
                ViewBag.message = "Prodcuto Creado";
            }
            else
            {
                await _productService.EditAsync(entityVm);
                ViewBag.message = "Producto editado";
            }

            return View(entityVm);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            await _productService.DeletedAsync(id);
            return RedirectToAction("Index"); 
        }
    }
}
