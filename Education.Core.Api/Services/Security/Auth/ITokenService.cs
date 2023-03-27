using Education.Core.Common.Dtos.User;

namespace Education.Core.Api.Services.Security.Auth
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        //string GetMobileAccessToken(MobileUserAuthDataDto userAuthData);
        string GetWebAccessToken(WebUserAuthDataDto userAuthData);
        bool IsTokenValid(string guid, string token);
        void RemoveToken(string guid);
    }
}
