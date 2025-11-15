using Microsoft.EntityFrameworkCore;
using MiTienda.Context;
using MiTienda.Entidades;

namespace MiTienda.Repositories
{
    public class OrderRepository : GenericRepository<Pedido>
    {
        private readonly AppDbContext _dbContext;
        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task AddAsync(Pedido pedido)
        {
            using var transaccion = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // restar stock , regitrar pedido , hacer la transaccion
                foreach (var detalles in pedido.Articulos)
                {
                    var producto = await _dbContext.Producto.FindAsync(detalles.ProductoId);
                    producto.Stock -= detalles.Cantidad;
                }

                await _dbContext.Pedido.AddAsync(pedido);
                // los cambios en Bd se guarden
                await _dbContext.SaveChangesAsync();

                await transaccion.CommitAsync();
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Pedido>> GetDetallesAsync(int userId)
        {
            var pedidos = await _dbContext.Pedido.Where(x => x.UserId == userId)
                .Include(x => x.Articulos).ThenInclude(x => x.Producto).ToListAsync();

            return pedidos;
        }

        public async Task<IEnumerable<Pedido>> GetAllDetallesAsync()
        {
            var pedidos = await _dbContext.Pedido
                .Include(x => x.Usuario)
                .Include(x => x.Articulos)
                    .ThenInclude(x => x.Producto)
                .ToListAsync();

            return pedidos;
        }

    }
}
