using Education.Core.Common.Dtos.Requests.Security;
using Education.Core.Common.Dtos.User;

namespace Education.Core.BLL.Interfaces
{
    public interface ISecurityService
    {
        Task<WebUserAuthDataDto> GetWebUserAuthDataByCredentialsAsync(WebUserLoginRequestDto request);
    }
}
