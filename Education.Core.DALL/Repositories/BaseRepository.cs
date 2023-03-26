using Education.Core.DALL.Intefaces.Repositories;

namespace Education.Core.DALL.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected ItalDBContext Context { get; }
        public BaseRepository(ItalDBContext context)
        {
            Context = context;
        }
    }
}
