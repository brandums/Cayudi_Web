using EPlatformWebApp.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPlatformWebApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context; 
            this._dbSet = _context.Set<T>();                   
        }

        public async Task create(T model)
        {
            await _dbSet.AddAsync(model);
            await saveToDatabase();
        }

        public async Task<List<T>> getAll()
        {
            IQueryable<T> query = _dbSet;
            return await query.ToListAsync();
        }

        public async Task<List<T>> getAllBy(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(filter);    
            return await query.ToListAsync();
        }

        public async Task<T> getFirstOrDefaultBy(Expression<Func<T, bool>> filter, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking(); 
            }
            return await query.Where(filter).FirstOrDefaultAsync();
        }

        public async Task <T> update(T model)
        {
            _context.Update(model);
            await saveToDatabase();
            return model;
        }

        public async Task Remove(T model)
        {
            _dbSet.Remove(model);
            await saveToDatabase();
        }

        public async Task saveToDatabase()
        {
            await _context.SaveChangesAsync();
        }       
    }
}
