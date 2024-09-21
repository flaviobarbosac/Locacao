using Locacao.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Locacao.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly Infraestructure.LocacaoDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(Infraestructure.LocacaoDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /* public async Task UpdateAsync(T entity)
         {
             _dbSet.Attach(entity);
             _context.Entry(entity).State = EntityState.Modified;
             await _context.SaveChangesAsync();


             var existingEntity = await _dbSet.FindAsync(entity.Id);
             if (existingEntity != null)
             {
                 _context.Entry(existingEntity).State = EntityState.Detached;
             }
             _context.Entry(entity).State = EntityState.Modified;
             await _context.SaveChangesAsync();

         } */

        public async Task UpdateAsync(T entity)
        {
            _context.ChangeTracker.Clear(); // This will detach all entities
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }       
    }
}
