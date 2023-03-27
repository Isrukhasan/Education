using System.Linq.Expressions;

namespace Education.Core.DALL.Intefaces.Repositories
{
    public interface IBaseRepository
    {
        public interface IBaseRepository<TModel> : IBaseRepository where TModel : class
        {
            Task<IEnumerable<TModel>> GetAllAsync();
            Task<IEnumerable<TModel>> GetAllAsync(Expression<Func<TModel, bool>> predicate);
            Task<TModel> GetByIdAsync(int id);
            void Insert(TModel dt);
            void InsertRange(IEnumerable<TModel> dt);
            void Update(TModel dt);
            void UpdateRange(IEnumerable<TModel> dt);
            void Update(int id, TModel dt);
            void Delete(TModel dt);
            Task SaveChangesAsync();
            Task<bool> AnyAsync();
            Task BulkInsertAsync(IEnumerable<TModel> items, string tableName);
        }
    }
}
