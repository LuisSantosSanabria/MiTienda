using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MiTienda.Entidades;
using MiTienda.Models;
using MiTienda.Repositories;
using System.Linq.Expressions;

namespace MiTienda.Services
{
    public class UsuarioService(GenericRepository<Usuario> _usuarioRepository)
    {
        public async Task<UsuarioVM> Login(LoginVM loginVM)
        {
            var condiciones = new List<Expression<Func<Usuario, bool>>>()
            {
                x => x.Email == loginVM.Email,
                x => x.Contrasena == loginVM.Clave,
            };

            var encontrado = await _usuarioRepository.GetByFilter(conditions: condiciones.ToArray());

            var usuarioVM = new UsuarioVM();
            if(encontrado != null)
            {
                usuarioVM.UserId = encontrado.UserId;
                usuarioVM.Nombre = encontrado.Nombre;
                usuarioVM.Email = encontrado.Email;
                usuarioVM.Tipo = encontrado.Tipo;
            }

            return usuarioVM;
        }
        //registro
        public async Task Registro(UsuarioVM usuarioVM)
        {
            if(usuarioVM.Clave != usuarioVM.RepetirClave)
                throw new InvalidOperationException("Las claves no son iguales");

            var condiciones = new List<Expression<Func<Usuario, bool>>>()
            {
                x => x.Email == usuarioVM.Email
            };

            var emailEncontrado = await _usuarioRepository.GetByFilter(conditions: condiciones.ToArray());

            if (emailEncontrado != null)
                throw new InvalidOperationException("El Email ya esta registrado");

            var entity = new Usuario()
            {
                Nombre = usuarioVM.Nombre,
                Email = usuarioVM.Email,
                Tipo = usuarioVM.Tipo,
                Contrasena = usuarioVM.Clave,
            };

            await _usuarioRepository.AddAsync(entity);
        }
    }
}
