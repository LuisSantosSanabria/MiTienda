using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace MiTienda.Controllers
{
    public class RegistroController(UsuarioService _usuarioService) : Controller
    {
        public IActionResult Login()
        {
            var viewModel = new LoginVM();
            return View(viewModel);
        }

        //devuelve la vista
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);
            var encontrado = await _usuarioService.Login(viewmodel);

            if (encontrado.UserId == 0)
            {
                ViewBag.message = "Error, revisa si el Email o Clave son correctos";
                return View();
            }
            else
            {
                // aplicamos aunt
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,encontrado.UserId.ToString()),
                    new Claim(ClaimTypes.Name,encontrado.Nombre),
                    new Claim(ClaimTypes.Email,encontrado.Email),
                    new Claim(ClaimTypes.Role,encontrado.Tipo),

                };

                // una identidad para todo lo de arriba
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties(){ AllowRefresh = true }
                    );

                if (encontrado.Tipo == "Adm")
                {
                    // Si es administrador → lo llevamos al panel
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    // Si es cliente → lo llevamos a la tienda normal
                    return RedirectToAction("Index", "Home");
                }

            }
        }
        public IActionResult Registro()
        {
            var viewModel = new UsuarioVM();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioVM viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);
            try
            {
                await _usuarioService.Registro(viewmodel);
                ViewBag.message = "Tu Cuenta fue Creada";
                ViewBag.Class = "alert-success";
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                ViewBag.Class = "alert-danger";
            }

            return View();
        }

        //validacion extra para que solo se pueda registar el adn mediante una clave especial

        //[HttpPost]
        //public async Task<IActionResult> Registro(UsuarioVM viewmodel)
        //{
        //    if (!ModelState.IsValid) return View(viewmodel);

        //    try
        //    {
        //        //si el usuario eligió ser administrador
        //        if (viewmodel.Tipo == "Adm")
        //        {
        //            // Recuperamos la contraseña especial desde el formulario
        //            string adminPassword = Request.Form["adminPasswordInput"];

        //            // Validamos la contraseña
        //            if (adminPassword != "TrabajoFinal2025")
        //            {
        //                ViewBag.message = "Contraseña de administrador incorrecta.";
        //                ViewBag.Class = "alert-danger";
        //                return View(viewmodel);
        //            }
        //        }

        //        //si esta todo ok creamos el usuario
        //        await _usuarioService.Registro(viewmodel);
        //        ViewBag.message = "Tu cuenta fue creada correctamente.";
        //        ViewBag.Class = "alert-success";
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.message = ex.Message;
        //        ViewBag.Class = "alert-danger";
        //    }

        //    return View(viewmodel);
        //}




        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
