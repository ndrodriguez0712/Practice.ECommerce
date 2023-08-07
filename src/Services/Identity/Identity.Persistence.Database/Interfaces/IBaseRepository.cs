using System.Linq.Expressions;

namespace Identity.Persistence.Database.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Returns a IQueryable of all objects in the database
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <returns>An IQueryable f every object in the database</returns>
        IQueryable<T> GetAllAsQueryable();

        /// <summary>
        /// Returns a single object which matches the provided expression
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A Linq expression filter to find a single result</param>
        /// <returns>A single object which matches the expression filter. 
        /// If more than one object is found or if zero are found, null is returned</returns>
        T Find(Expression<Func<T, bool>> match);

        /// <summary>
        /// Returns first or default object which matches the provided expression
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A Linq expression filter to find a single result</param>
        /// <returns>First or default  object which matches the expression filter. 
        /// If more than one object is found or if zero are found, null is returned</returns>
        T FindFirstOrDefault(Expression<Func<T, bool>> match);

        /// <summary>
        /// Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An List of object which match the expression filter</returns>
        List<T> FindAll(Expression<Func<T, bool>> match);

        /// <summary>
        /// Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An List of object which match the expression filter</returns>
        IEnumerable<T> Get(Expression<Func<T, bool>> match);

        /// <summary>
        /// Returns a IQueryable of objects which match the provided expression
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An IQueryable of object which match the expression filter</returns>
        IQueryable<T> FindAllAsQueryable(Expression<Func<T, bool>> match);

        /// <summary>
        /// Inserts a single object to the database 
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="t">The object to insert</param>
        /// <returns>The resulting object including its primary key after the insert</returns>
        T Add(T t);

        /// <summary>
        /// Inserts a collection of objects into the database 
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="tList">An IEnumerable list of objects to insert</param>
        /// <returns>The IEnumerable resulting list of inserted objects including the primary keys</returns>
        IEnumerable<T> AddAll(IEnumerable<T> tList);

        /// <summary>
        /// Updates a single object based on the provided primary key 
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="updated">The updated object to apply to the database</param>
        /// <param name="key">The primary key of the object to update</param>
        /// <returns>The resulting updated object</returns>
        T Update(T updated, int key);

        /// <summary>
        /// Deletes a single object from the database 
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="t">The object to delete</param>
        void Delete(T t);

        /// <summary>
        /// Deletes a collection of objects from the database 
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="tList">Collection of objects to delete</param>
        void DeleteRange(IEnumerable<T> tList);

        /// <summary>
        /// Delete objects which match the provided expression. 
        /// If not provided, deletes all objects from the database.
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        void DeleteAll(Expression<Func<T, bool>> match = null);

        /// <summary>
        /// Gets the count of the number of objects which match the provided expression. 
        /// If not provided, count all objects in the database.
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>The count of the number of objects</returns>
        int Count(Expression<Func<T, bool>> match = null);


        /// <summary>
        /// Get if the element exists
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>The count of the number of objects</returns>
        bool Exists(Expression<Func<T, bool>> match = null);
    }
}
