using Education.Core.DALL.QueryModels.Security;
using Microsoft.EntityFrameworkCore;

namespace Education.Core.DALL;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    #region Stored_Procedures_Query_DbSets
    public DbSet<WebUserAuthDataQm> WebUserAuthDataQm { get; set; }
    #endregion
}