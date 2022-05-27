using ArmyTechTask.DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArmyTechTask.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Delete(int id) => _dbSet.Remove(await _dbSet.FindAsync(id));

        public async Task Delete(long id) => _dbSet.Remove(await _dbSet.FindAsync(id));

        public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);


        public async Task<T> Get(
            Expression<Func<T, bool>> expression = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
            )
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
                query = include(query);
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;
            if (expression != null)
                query = query.Where(expression);
            if (include != null)
                query = include(query);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task Insert(T entity) => await _dbSet.AddAsync(entity);

        public async Task InsertRange(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.AttachRange(entities);
            _context.Entry(entities).State = EntityState.Modified;
        }
    }
}
