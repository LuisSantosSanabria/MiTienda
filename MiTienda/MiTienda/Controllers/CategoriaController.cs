using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiTienda.Models;
using MiTienda.Services;


namespace MiTienda.Controllers
{
    public class CategoriaController(CategoriaService _categoriaService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            //obtenr toda la lista de categorias
            var categorias =await _categoriaService.GetAllAsync();
            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var categoriaVM = await _categoriaService.GetByIdAsync(id);
            return View(categoriaVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoriaVM entityVm)
        {
            ViewBag.message = null;
            if (!ModelState.IsValid) return View(entityVm);

            if(entityVm.CategoriaId == 0)
            {
                await _categoriaService.AddAsync(entityVm);
                ModelState.Clear();
                entityVm = new CategoriaVM();
                ViewBag.message = "Categoria Creada";
            }
            else
            {
                await _categoriaService.EditAsync(entityVm);
                ViewBag.message = "Categoria editada";
            }

                return View(entityVm);
        }

        public async Task<IActionResult>Eliminar(int id)
        {
            await _categoriaService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
