namespace Education.Core.Common.Options
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifetimeMinutes { get; set; }
        public int LifetimeMonths { get; set; }
    }
}
