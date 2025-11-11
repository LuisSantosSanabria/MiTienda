using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiTienda.Controllers
{
    [Authorize(Roles = "Adm")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Panel Administrativo";
            return View();
        }
    }
}
