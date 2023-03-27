using Education.Core.DALL.Intefaces.Repositories;
using Education.Core.DALL.QueryModels.Security;
using Microsoft.EntityFrameworkCore;

namespace Education.Core.DALL.Repositories
{
    public class SecurityRepository : BaseRepository, ISecurityRepository
    {
        public SecurityRepository(AuthDbContext context) : base(context)
        {
        }
        /// <summary>
        /// Method getting User authentication data by credentials for login to application
        /// </summary>
        /// <param name="model">Dto contains properties needed to get specific User authentication data</param>
        /// <returns></returns>
        public async Task<WebUserAuthDataQm> GetWebUserAuthDataByCredentialsAsync(string username)
        {
            var users = await Context.WebUserAuthDataQm
                .FromSqlInterpolated($"EXEC dbo.GetWebUserAuthDataByCredentials_sp {username}")
                .AsNoTracking()
                .ToListAsync();

            return users.FirstOrDefault();
        }
    }
}

