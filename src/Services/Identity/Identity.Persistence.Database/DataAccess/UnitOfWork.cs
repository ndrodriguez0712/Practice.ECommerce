using Identity.Persistence.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Database.DataAccess
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        #region Variables
        private Dictionary<Type, object> _repositories;
        private bool isDisposed;
        #endregion

        #region Constructor
        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new BaseRepository<TEntity>(Context);
            return (IBaseRepository<TEntity>)_repositories[type];
        }

        public TContext Context { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                Context?.Dispose();
            }
            isDisposed = true;
        }        
    }
}
