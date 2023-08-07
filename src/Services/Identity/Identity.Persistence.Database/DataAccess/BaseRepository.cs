using Identity.Persistence.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Identity.Persistence.Database.DataAccess
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Variables
        private readonly DbContext _context;
        #endregion

        #region Constructor
        public BaseRepository(DbContext context)
        {
            _context = context;
        }
        #endregion

        public IQueryable<T> GetAllAsQueryable() => _context.Set<T>();

        public T SingleOrDefault(Expression<Func<T, bool>> match) => _context.Set<T>().SingleOrDefault(match);        

        public T FirstOrDefault(Expression<Func<T, bool>> match) => _context.Set<T>().FirstOrDefault(match);

        public List<T> FindAll(Expression<Func<T, bool>> match) => _context.Set<T>().Where(match).ToList();

        public IEnumerable<T> Get(Expression<Func<T, bool>> match) => _context.Set<T>().Where(match);

        public IQueryable<T> FindAllAsQueryable(Expression<Func<T, bool>> match) => _context.Set<T>().Where(match);

        public T Add(T t)
        {
            _context.Set<T>().Add(t);
            return t;
        }

        public IEnumerable<T> AddAll(IEnumerable<T> tList)
        {
            _context.Set<T>().AddRange(tList);
            return tList;
        }

        public T Update(T updated, int key)
        {
            if (updated == null)
                return null;

            T existing = _context.Set<T>().Find(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }

        public void Delete(T t)
        {
            _context.Set<T>().Remove(t);
        }

        public void DeleteRange(IEnumerable<T> tList)
        {
            _context.Set<T>().RemoveRange(tList);
        }

        public void DeleteAll(Expression<Func<T, bool>> match = null)
        {
            if (match != null)
            {
                var tList = _context.Set<T>().Where(match);
                _context.Set<T>().RemoveRange(tList);
            }
            else
            {
                var tList = _context.Set<T>();
                _context.Set<T>().RemoveRange(tList);
            }
        }

        public int Count(Expression<Func<T, bool>> match = null) => match != null ? _context.Set<T>().Count(match) : _context.Set<T>().Count();

        public bool Exists(Expression<Func<T, bool>> match = null) => _context.Set<T>().Any(match);

        public async Task<T> AddAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
            return t;
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> match) => await _context.Set<T>().SingleOrDefaultAsync(match);

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> match) => await _context.Set<T>().FirstOrDefaultAsync(match);

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match) => await _context.Set<T>().Where(match).ToListAsync();        
    }
}
