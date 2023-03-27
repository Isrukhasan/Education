namespace Education.Core.Api.Services.Security.Auth
{
    /// <summary>
    /// Jwt custom claim types
    /// </summary>
    public static class JwtClaimTypes
    {
        public const string USER_ID = "user_id";
        public const string CACHE_TOKEN_ID = "cache_token_id";
        public const string CLIENT_ID = "client_id";
        public const string BADGE_ID = "badge_id";
        public const string SERIAL_NO = "serial_no";
        public const string PAF_FILE_LOCATION = "paf_data_file_location";
        public const string LOGGING_LEVEL = "logging_level";
    }
}
