using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using WebGoatCore.Data;
using Microsoft.Data.Sqlite;
using System;

namespace WebGoat.NET.Data
{
    public class NorthwindContextDesignTimeDbContextFactory
         : IDesignTimeDbContextFactory<NorthwindContext>
    {
        public NorthwindContext CreateDbContext(string[] args)
        {
            try
            {
                // Load configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // Ensure base path is correct
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Ensure appsettings.json exists
                    .Build();

                var execDirectory = configuration.GetValue<string>(Constants.WEBGOAT_ROOT);

                // Check if the execDirectory is null or empty
                if (string.IsNullOrEmpty(execDirectory))
                {
                    throw new InvalidOperationException("The WEBGOAT_ROOT value is not set in the configuration.");
                }

                Console.WriteLine($"Exec Directory: {execDirectory}"); // Debugging log

                var builder = new SqliteConnectionStringBuilder();
                builder.DataSource = Path.Combine(execDirectory, "NORTHWND.sqlite");
                var connString = builder.ConnectionString;

                if (string.IsNullOrEmpty(connString))
                {
                    throw new WebGoatCore.Exceptions.WebGoatStartupException("Cannot compute connection string to connect database!");
                }

                // Set up DbContext options
                var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
                optionsBuilder.UseSqlite(connString); // Use SQLite with the connection string

                return new NorthwindContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Rethrow exception to propagate the error
            }
        }

    }
}
