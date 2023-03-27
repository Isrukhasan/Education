namespace Education.Core.BLL.Interfaces
{
    public interface IBCryptService
    {
        string HashPassword(string password);
        bool VerifyPasswords(string password, string hashedPassword);
    }
}
