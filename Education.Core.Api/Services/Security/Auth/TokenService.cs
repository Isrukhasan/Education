using Education.Core.Common.Dtos.User;
using Education.Core.Common.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Education.Core.Api.Services.Security.Auth
{
    /// <summary>
    /// Service class for generating and store access tokens
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IMemoryCache _cache;
        private static string _cachePrefix = "token_";

        public TokenService(IOptions<JwtSettings> jwtSettings, IMemoryCache cache)
        {
            _jwtSettings = jwtSettings.Value;
            _cache = cache;
        }

        #region Private_Methods
        private string CalculateMd5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private string GetCacheKey(string guid)
        {
            return string.Concat(_cachePrefix, guid);
        }

        private string WriteAccessTokenBasedOnClaims(List<Claim> claims, string guid)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.LifetimeMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var hash = CalculateMd5Hash(accessToken);
            _cache.Set(GetCacheKey(guid), hash);

            return accessToken;
        }
        #endregion

        #region Public_Methods
        //public string GetMobileAccessToken(MobileUserAuthDataDto user)
        //{
        //    var guid = Guid.NewGuid().ToString();

        //    var pafLocation = user.PafDataFileLocation ?? string.Empty;

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtClaimTypes.CACHE_TOKEN_ID, guid),
        //        new Claim(JwtClaimTypes.USER_ID, user.UserId.ToString()),
        //        new Claim(JwtClaimTypes.CLIENT_ID, user.ClientId.ToString()),
        //        new Claim(JwtClaimTypes.BADGE_ID, user.BadgeId.ToString()),
        //        new Claim(JwtClaimTypes.SERIAL_NO, user.SerialNumber),
        //        new Claim(JwtClaimTypes.PAF_FILE_LOCATION, pafLocation),
        //        new Claim(JwtClaimTypes.LOGGING_LEVEL, user.LoggingLevel.ToString()),
        //        new Claim(ClaimTypes.Name, user.Username.ToString())
        //    };

        //    var accessToken = WriteAccessTokenBasedOnClaims(claims, guid);

        //    return accessToken;
        //}

        public string GetWebAccessToken(WebUserAuthDataDto user)
        {
            var guid = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.CACHE_TOKEN_ID, guid),
                new Claim(JwtClaimTypes.USER_ID, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username.ToString())
            };

            var accessToken = WriteAccessTokenBasedOnClaims(claims, guid);

            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public bool IsTokenValid(string guid, string token)
        {
            var cacheExists = _cache.TryGetValue(GetCacheKey(guid), out var cacheHash);
            if (!cacheExists || !(cacheHash is string))
            {
                return false;
            }

            var cacheHashString = (string)cacheHash;
            var hash = CalculateMd5Hash(token);
            return hash.Equals(cacheHashString);
        }

        public void RemoveToken(string guid)
        {
            _cache.Remove(GetCacheKey(guid));
        }
        #endregion
    }
}
