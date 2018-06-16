using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace MoeLib.Data.Core
{
    /// <summary>
    ///     A <see cref="DbContextBase" /> adds Auto-Retry function for DbContext of EntityFramework.
    /// </summary>
    /// <remarks>
    ///     建议使用MoeDbContext作为业务中使用的DbContext的基类，MoeDbContext为DbContext的常用操作添加了自动重试机制。
    /// </remarks>
    public abstract class DbContextBase : DbContext
    {
        private readonly RetryPolicy retryPolicy;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbContextBase" /> class.
        /// </summary>
        /// <param name="nameOrConnectionString">The name of the connection string setting or connection string.</param>
        protected DbContextBase(DbContextOptions _options)
            : base(_options)
        {
            this.retryPolicy = new RetryPolicy(new SqlDatabaseTransientErrorDetectionStrategy(), RetryStrategy.DefaultExponential);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbContextBase" /> class.
        /// </summary>
        /// <param name="nameOrConnectionString">The name of the connection string setting or connection string.</param>
        /// <param name="retryPolicy">The retry policy used by the <see cref="DbContextBase" /> instance.</param>
        protected DbContextBase(DbContextOptions _options, RetryPolicy retryPolicy)
            : base(_options)
        {
            this.retryPolicy = retryPolicy;
        }

        /// <summary>
        ///     Adds the specified entity to this context.
        /// </summary>
        /// <typeparam name="T">The type of the adding entity.</typeparam>
        /// <param name="entity">The adding entity.</param>
        public void Add<T>(T entity) where T : class
        {
            EntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.Set<T>().Add(entity);
            }
        }

        /// <summary>
        ///     Adds the specified entities to this context.
        /// </summary>
        /// <typeparam name="T">The type of the adding entities.</typeparam>
        /// <param name="entities">The adding entities.</param>
        public void Add<T>(IEnumerable<T> entities) where T : class
        {
            foreach (T entity in entities)
            {
                EntityEntry<T> entry = this.Entry(entity);

                if (entry.State == EntityState.Detached)
                {
                    this.Set<T>().Add(entity);
                }
            }
        }

        /// <summary>
        ///     Deletes the specified entity form this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the deleting entity.</typeparam>
        /// <param name="entity">The deleting entity.</param>
        /// <returns>
        ///     The number of entries deleted from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public int Delete<T>(T entity) where T : class
        {
            this.Entry(entity).State = EntityState.Deleted;

            return this.ExecuteSaveChanges();
        }

        /// <summary>
        ///     Asynchronously deletes the specified entity form this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the deleting entity.</typeparam>
        /// <param name="entity">The deleting entity.</param>
        /// <returns>
        ///     A task that represents the asynchronous delete operation.
        ///     The task result contains the number of entries deleted from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public Task<int> DeleteAsync<T>(T entity) where T : class
        {
            this.Entry(entity).State = EntityState.Deleted;

            return this.ExecuteSaveChangesAsync();
        }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        ///     The number of state entries written to the underlying database. This can include
        ///     state entries for entities and/or relationships. Relationship state entries are created for
        ///     many-to-many relationships and relationships where there is no foreign key property
        ///     included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public int ExecuteSaveChanges()
        {
            return this.ExecuteAction(this.SaveChanges);
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <param name="cancellationToken">
        ///     A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of state entries written to the underlying database. This can include
        ///     state entries for entities and/or relationships. Relationship state entries are created for
        ///     many-to-many relationships and relationships where there is no foreign key property
        ///     included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public Task<int> ExecuteSaveChangesAsync(CancellationToken cancellationToken)
        {
            return this.ExecuteAsync(() => this.SaveChangesAsync(cancellationToken), cancellationToken);
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of state entries written to the underlying database. This can include
        ///     state entries for entities and/or relationships. Relationship state entries are created for
        ///     many-to-many relationships and relationships where there is no foreign key property
        ///     included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public Task<int> ExecuteSaveChangesAsync()
        {
            return this.ExecuteAsync<int>(() => this.SaveChangesAsync());
        }


        /// <summary>
        ///     Returns a <see cref="T:System.Data.Entity.DbSet`1" /> instance for access to entities of the given type in the context and the underlying store.
        /// </summary>
        /// <remarks>
        ///     Note that Entity Framework requires that this method return the same instance each time that it is called
        ///     for a given context instance and entity type. Also, the non-generic <see cref="T:System.Data.Entity.DbSet" /> returned by the
        ///     <see cref="M:System.Data.Entity.DbContext.Set(System.Type)" /> method must wrap the same underlying query and set of entities. These invariants must
        ///     be maintained if this method is overridden for anything other than creating test doubles for unit testing.
        ///     See the <see cref="T:System.Data.Entity.DbSet`1" /> class for more details.
        /// </remarks>
        /// <typeparam name="T">The type entity for which a set should be returned. </typeparam>
        /// <returns>
        ///     A set for the given entity type.
        /// </returns>
        public DbSet<T> Query<T>() where T : class
        {
            return this.Set<T>();
        }

        /// <summary>
        ///     Returns a new query where the entities returned will not be cached in the <see cref="T:System.Data.Entity.DbContext" />.
        /// </summary>
        /// <typeparam name="T">The type entity for which a query should be returned. </typeparam>
        /// <returns>
        ///     A new query with NoTracking applied.
        /// </returns>
        public IQueryable<T> ReadonlyQuery<T>() where T : class
        {
            return this.Set<T>().AsNoTracking();
        }

        /// <summary>
        ///     Removes the specified entity from this context.
        /// </summary>
        /// <typeparam name="T">The type of the removing entity.</typeparam>
        /// <param name="entity">The removing entity.</param>
        public void Remove<T>(T entity) where T : class
        {
            this.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        ///     Saves the specified entity to this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the saving entity.</typeparam>
        /// <param name="entity">The saving entity.</param>
        /// <returns>
        ///     The number of entries saved to the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public int Save<T>(T entity) where T : class
        {
            EntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);

            return this.ExecuteSaveChanges();
        }

        /// <summary>
        ///     Asynchronously saves the specified entity to this context and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the saving entity.</typeparam>
        /// <param name="entity">The saving entity.</param>
        /// <returns>
        ///     A task that represents the asynchronous delete operation.
        ///     The task result contains the number of entries deleted from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public Task<int> SaveAsync<T>(T entity) where T : class
        {
            EntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);

            return this.ExecuteSaveChangesAsync();
        }

        /// <summary>
        ///     Saves the specified entity to this context or updates the entity in this context, and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to save or update.</param>
        /// <param name="identifierExpression">The identifier expression for determining whether the entity is exsting in the underlying database.</param>
        /// <returns>
        ///     The number of entries saved or updated to the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public int SaveOrUpdate<T>(T entity, Expression<Func<T, bool>> identifierExpression) where T : class
        {
            EntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);

            if (this.Set<T>().Any(identifierExpression))
            {
                entry.State = EntityState.Modified;
            }

            return this.ExecuteSaveChanges();
        }

        /// <summary>
        ///     Asynchronously saves the specified entity to this context or updates the entity in this context, and saves all changes made in this context to the underlying database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to save or update.</param>
        /// <param name="identifierExpression">The identifier expression for determining whether the entity is exsting in the underlying database.</param>
        /// <returns>
        ///     A task that represents the asynchronous save or update operation.
        ///     The task result contains the number of entries saved or updated from the underlying database.
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///     on the same context instance.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either before or after sending commands
        ///     to the database.
        /// </exception>
        public Task<int> SaveOrUpdateAsync<T>(T entity, Expression<Func<T, bool>> identifierExpression) where T : class
        {
            EntityEntry<T> entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);

            if (this.Set<T>().Any(identifierExpression))
            {
                entry.State = EntityState.Modified;
            }

            return this.ExecuteSaveChangesAsync();
        }

        private TResult ExecuteAction<TResult>(Func<TResult> func)
        {
            return this.retryPolicy.ExecuteAction(func);
        }

        private Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc)
        {
            return this.retryPolicy.ExecuteAsync(taskFunc);
        }

        private Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc, CancellationToken cancellationToken)
        {
            return this.retryPolicy.ExecuteAsync(taskFunc, cancellationToken);
        }
    }
}