namespace Education.Core.Common.Dtos.Requests.Security
{
    /// <summary>
    /// Model class for user login request data.
    /// </summary>
    public class WebUserLoginRequestDto
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}


