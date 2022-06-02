using HotelsListing.Data;
using HotelsListing.Repository_Pattern.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelsListing.Repository_Pattern.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly DataBaseContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(DataBaseContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }
        public async Task Delete(int Id)
        {
            var entity = await _db.FindAsync(Id);
            _db.Remove(entity);
        }

        public void DeleteEntitiesRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null) {
                foreach (var IncludeProperty in includes) { 
                   query = query.Include(IncludeProperty);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (expression != null) {
               query = query.Where(expression);
            }
            if (includes != null) {
                foreach (var IncludeProperty in includes) {
                    query = query.Include(IncludeProperty);
                    }
               }
            if (orderby != null) {
                query = orderby(query);
            }
            return await query.AsNoTracking().ToListAsync();  
        }

        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity); // _context.Set<T>().AddAsync(entity)
        }

        public async Task InsertEntitiesRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
