using Microsoft.EntityFrameworkCore;
using MiTienda.Context;
using System.Linq.Expressions;

namespace MiTienda.Repositories
{
    public class GenericRepository<TEntity>(AppDbContext _dbContext) where TEntity : class
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        //sirve para obtener todos los registros de un tipo de entidad de forma asíncrona
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>[]? conditions = null,
            Expression<Func<TEntity, object>>[]? includes = null
            )
        //sirve para obtener los dif productos con otra entidad
        {
            // la linea de abajo es un select*Producto
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (conditions is not null)
                foreach (var condition in conditions) query = query.Where(condition);

            if (includes is not null)
            foreach (var include in includes) query = query.Include(include);

            return await query.ToListAsync();
        }

        //virtual para sobreescribir por clases hijas como OrderRepository
        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity?>GetByIdAsync(int entityId)
        {
            return await _dbContext.Set<TEntity>().FindAsync(entityId);
        }

        public async Task EditAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Usuario existe?
        public async Task<TEntity?> GetByFilter(
            Expression<Func<TEntity, bool>>[] conditions)
        //sirve para obtener los dif productos con otra entidad
        {
            // la linea de abajo es un select*Producto
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (conditions is not null)
                foreach (var condition in conditions) query = query.Where(condition);

            return await query.FirstOrDefaultAsync(); 
        }

    }
}
