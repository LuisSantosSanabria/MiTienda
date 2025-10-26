using Microsoft.EntityFrameworkCore;
using MiTienda.Entidades;
using MiTienda.Models;
using MiTienda.Repositories;

namespace MiTienda.Services
{
    public class CategoriaService(GenericRepository<Categoria> _categoriaRepository)
    {
        public async Task<IEnumerable<CategoriaVM>> GetAllAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            // vamos a obtenr las cat de la Db

            var categoriasVM = categorias.Select(item =>
            new CategoriaVM
            {
                CategoriaId = item.CategoriaId,
                Nombre = item.Nombre,
            }).ToList();

            return categoriasVM;
        }

        public async Task AddAsync(CategoriaVM viewModel)
        {
            var entity = new Categoria
            {
                Nombre = viewModel.Nombre,
            };
            await _categoriaRepository.AddAsync(entity);
        }

        public async Task<CategoriaVM?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            var categoriaVM = new CategoriaVM();

            if(categoria != null)
            {
                categoriaVM.Nombre = categoria.Nombre;
                categoriaVM.CategoriaId = categoria.CategoriaId;
            }

            return categoriaVM;
        }

        public async Task EditAsync(CategoriaVM viewModel)
        {
            var entity = new Categoria
            {
                CategoriaId = viewModel.CategoriaId,
                Nombre = viewModel.Nombre,
            };
            await _categoriaRepository.EditAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
           var categoria = await _categoriaRepository.GetByIdAsync(id);
            await _categoriaRepository.DeletAsync(categoria!);
        }

    }
}
