namespace Education.Core.Common.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public bool IsSuspended { get; set; }
        public string BadgeId { get; set; }
        public string RefreshToken { get; set; }
        public bool IsDeleted { get; set; }
        public int? UpdatedByUserId { get; set; }
    }
}
