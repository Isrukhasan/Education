namespace Education.Core.Common.Dtos.Security
{
    public class GetAccessTokenRequestDto
    {
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public string SerialNumber { get; set; }
        public string Version { get; set; }
        public string Build { get; set; }
    }
}
