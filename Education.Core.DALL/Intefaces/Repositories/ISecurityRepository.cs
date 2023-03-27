using Education.Core.DALL.QueryModels.Security;

namespace Education.Core.DALL.Intefaces.Repositories
{
    public interface ISecurityRepository : IBaseRepository
    {

        Task<WebUserAuthDataQm> GetWebUserAuthDataByCredentialsAsync(string username);
    }
}
