namespace Education.Core.Common.Dtos.User
{
    public class WebUserAuthDataDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
