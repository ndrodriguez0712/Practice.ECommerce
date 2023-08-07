namespace Identity.Persistence.Database.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable
    {
        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
        TContext Context { get; }

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns>The number of state entries written to the underlying database</returns>
        int SaveChanges();
    }
}
