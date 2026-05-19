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

            // Default connection string for local development using SQLite.
            var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var appFolder = System.IO.Path.Combine(folder, "TheMatchaClub");
            System.IO.Directory.CreateDirectory(appFolder);
            var dbPath = System.IO.Path.Combine(appFolder, "TheMatchaClub.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new MatchaClubDbContext(optionsBuilder.Options);
        }
    }
}
