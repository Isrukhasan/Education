using Education.Core.DALL.Intefaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Education.Core.DALL.Intefaces.Repositories.IBaseRepository;

namespace Education.Core.DALL.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected AuthDbContext Context { get; }
        public BaseRepository(AuthDbContext context)
        {
            Context = context;
        }
    }
    /// <summary>
    /// Base class for repositories contains implementation of BeginTransaction method and basic operation on DB context
    /// </summary>
    /// <typeparam name="TModel">TModel it's a repository class</typeparam>
    public abstract class BaseRepository<TModel> : BaseRepository, IBaseRepository<TModel> where TModel : class
    {
        protected readonly string _connectionString = string.Empty;


        public BaseRepository(AuthDbContext context) : base(context)
        {
            _connectionString = context.Database.GetDbConnection().ConnectionString;

            Context.Database.SetCommandTimeout(TimeSpan.FromMinutes(15));
        }

        /// <summary>
        /// Basic implementation of delete from database
        /// </summary>
        /// <param name="dt">Repository entity</param>
        public virtual void Delete(TModel dt)
        {
            Context.Set<TModel>().Remove(dt);
        }

        /// <summary>
        /// Basic implementation of get all repository entity records
        /// </summary>
        /// <returns>Full list of repository entity records</returns>
        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await Context.Set<TModel>().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Basic implementation of find all records fit to specific predicate
        /// </summary>
        /// <param name="predicate">Expression used to filter records</param>
        /// <returns>Filtered list of repository entity</returns>
        public virtual async Task<IEnumerable<TModel>> GetAllAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await Context.Set<TModel>().Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Basic implementation of find record by identity 
        /// </summary>
        /// <param name="id">Identity of an entity</param>
        /// <returns>Entity object</returns>
        public virtual async Task<TModel> GetByIdAsync(int id)
        {
            var obj = await Context.Set<TModel>().FindAsync(id);
            Context.Entry(obj).State = EntityState.Detached;

            return obj;
        }

        /// <summary>
        /// Basic implementation of insert record to database
        /// </summary>
        /// <param name="dt">Model class</param>
        public virtual void Insert(TModel dt)
        {
            Context.Set<TModel>().Add(dt);
        }

        /// <summary>
        /// Basic implementation of insert all records to database
        /// </summary>
        /// <param name="dt">List of entities</param>
        public virtual void InsertRange(IEnumerable<TModel> dt)
        {
            Context.Set<TModel>().AddRange(dt);
        }

        /// <summary>
        /// Basic implementation of context save changes 
        /// </summary>
        /// <returns></returns>
        public virtual async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Basic implementation update model 
        /// </summary>
        /// <param name="dt">Model class</param>
        public virtual void Update(TModel dt)
        {
            Context.Set<TModel>().Update(dt);
        }

        /// <summary>
        /// Sync implementation update model - EF keeps track of items that were selected before update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dt"></param>
        public virtual void Update(int id, TModel dt)
        {
            TModel obj = Context.Set<TModel>().Find(id);
            Context.Entry(obj).CurrentValues.SetValues(dt);
        }

        /// <summary>
        /// Basic implementation of update all records to database
        /// </summary>
        /// <param name="dt">List of entities</param>
        public void UpdateRange(IEnumerable<TModel> dt)
        {
            Context.Set<TModel>().UpdateRange(dt);
        }

        /// <summary>
        /// Basic implementation of check if there's any element in database
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync()
        {
            return await Context.Set<TModel>().AnyAsync();
        }

        public virtual Task BulkInsertAsync(IEnumerable<TModel> items, string tableName)
        {
            return Task.CompletedTask;
        }
    }
}

