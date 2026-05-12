using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TheMatchaClub.Infrastructure.Data
{
    /// <summary>
    /// Design-time factory used by EF Core CLI tools (dotnet ef migrations add, etc.)
    /// to create a MatchaClubDbContext without needing to run the WinForms app.
    /// 
    /// Update the connection string below to point to your SQL Server instance.
    /// </summary>
    public class MatchaClubDbContextFactory : IDesignTimeDbContextFactory<MatchaClubDbContext>
    {
        public MatchaClubDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MatchaClubDbContext>();

            // Default connection string for local development.
            // Change "Server" to your SQL Server instance name (e.g., ".\\SQLEXPRESS" or "localhost").
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=TheMatchaClubDb;Trusted_Connection=True;");

            return new MatchaClubDbContext(optionsBuilder.Options);
        }
    }
}
