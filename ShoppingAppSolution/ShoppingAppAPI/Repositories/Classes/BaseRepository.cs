using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public abstract class BaseRepository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly ShoppingAppContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ShoppingAppContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> Add(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Delete(K id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"{id} not found.");
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public abstract Task<T> Get(K key);

        public virtual async Task<IEnumerable<T>> Get()
        {
            var result = await _dbSet.ToListAsync();
            return result;
        }
    }
}
