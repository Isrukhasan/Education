using Education.Core.BLL.Interfaces;
using Education.Core.DALL.Options;
using Microsoft.Extensions.Options;

namespace Education.Core.BLL.Services
{
    public class BCryptService : IBCryptService
    {
        private readonly BCryptSettings _bCryptSettings;

        public BCryptService(IOptions<BCryptSettings> config)
        {
            _bCryptSettings = config.Value;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, _bCryptSettings.BCryptWorkFactor);
        }

        public bool VerifyPasswords(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
