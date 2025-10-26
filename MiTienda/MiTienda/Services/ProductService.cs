using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MiTienda.Entidades;
using MiTienda.Models;
using MiTienda.Repositories;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiTienda.Services
{
    public class ProductService
        (GenericRepository<Categoria> _categoriaRepository,
         GenericRepository<Producto> _productRepository,
         IWebHostEnvironment _webHostEnvironment)
    //nos permite enviar las img a las carpteas dentro de nuestro proyecto
    {
        public async Task<IEnumerable<ProductoVM>> GetAllAsync()
        {
            var productos = await _productRepository.GetAllAsync(
                includes: new Expression<Func<Producto, object>>[] { x => x.Categoria! });

            var productsVm = productos.Select(item =>
            new ProductoVM
            {
                ProdcutoId = item.ProdcutoId,
                Categoria = new CategoriaVM
                {
                    CategoriaId = item.Categoria!.CategoriaId,
                    Nombre = item.Categoria!.Nombre,
                },
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Stock = item.Stock,
                ImgNombre = item.ImgNombre,
            }).ToList();

            return productsVm;
        }

        //devolcer producto por id
        public async Task<ProductoVM> GetByIdAsync(int id)
        {
            // Obtener el producto de la DB
            var producto = await _productRepository.GetByIdAsync(id);
            var categorias = await _categoriaRepository.GetAllAsync();

            var productVM = new ProductoVM();

            if (producto != null)
            {
                productVM = new ProductoVM
                {
                    ProdcutoId = producto.ProdcutoId,
                    Categoria = new CategoriaVM
                    {
                        CategoriaId = producto!.CategoriaId,
                        Nombre = producto.Categoria.Nombre,
                    },
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    ImgNombre = producto.ImgNombre,
                };

            }

                productVM.Categorias = categorias.Select(item => new SelectListItem
                {
                    Value = item.CategoriaId.ToString(),
                    Text = item.Nombre,
                }).ToList();

            return productVM;
        }


        // agregar
        public async Task AddAsync(ProductoVM viewModel)
        {
            if (viewModel.ImageFile != null)
            {
                // carpteda dnd se va subir las imagens
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImageFile.FileName);
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create)) await viewModel.ImageFile.CopyToAsync(fileStream);

                viewModel.ImgNombre = uniqueFileName;
            }
            var entity = new Producto
            {
                CategoriaId = viewModel.Categoria.CategoriaId,
                Nombre = viewModel.Nombre,
                Descripcion = viewModel.Descripcion,
                Precio = viewModel.Precio,
                Stock = viewModel.Stock,
                ImgNombre = viewModel.ImgNombre,
            };

            await _productRepository.AddAsync(entity);
        }

        //editar
        public async Task EditAsync(ProductoVM viewModel)
        {
            var producto = await _productRepository.GetByIdAsync(viewModel.ProdcutoId);

            if (viewModel.ImageFile != null)
            {
                //guardar la img existe
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImageFile.FileName);
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create)) await viewModel.ImageFile.CopyToAsync(fileStream);

                if (!producto.ImgNombre.IsNullOrEmpty())
                {
                    var previusImg = producto.ImgNombre;
                    string deleteFilePath = Path.Combine(uploadFolder, previusImg);

                    if (File.Exists(deleteFilePath)) File.Delete(deleteFilePath);
                }
                viewModel.ImgNombre = uniqueFileName;
            }
            else
            {
                viewModel.ImgNombre = producto.ImgNombre;
            }

            producto.CategoriaId = viewModel.Categoria.CategoriaId;
            producto.Nombre = viewModel.Nombre;
            producto.Descripcion = viewModel.Descripcion;
            producto.Precio = viewModel.Precio;
            producto.Stock = viewModel.Stock;
            producto.ImgNombre = viewModel.ImgNombre;

            await _productRepository.EditAsync(producto);
        }

        //eliminar
        public async Task DeletedAsync(int id) {
            var producto = await _productRepository.GetByIdAsync(id);

            await _productRepository.DeletAsync(producto!);
        }

        public async Task<IEnumerable<ProductoVM>> GetCatalogoAsync(int categoriaId=0, string buscar="")
        {

            var condiciones = new List<Expression<Func<Producto, bool>>>
            {
                x => x.Stock > 0
            };

            if (categoriaId != 0) condiciones.Add(x => x.CategoriaId == categoriaId);
            if (string.IsNullOrEmpty(buscar)) condiciones.Add(x => x.Nombre.Contains(buscar));

            var productos = await _productRepository.GetAllAsync(conditions : condiciones.ToArray());

            var productsVm = productos.Select(item =>
            new ProductoVM
            {
                ProdcutoId = item.ProdcutoId,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Stock = item.Stock,
                ImgNombre = item.ImgNombre,
            }).ToList();

            return productsVm;
        }
    }
}
