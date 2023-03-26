namespace Education.Core.Common.Dtos.User
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public bool IsVersionUpgradeRequired { get; set; }
    }
}
