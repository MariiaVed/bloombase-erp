using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bloombase
{
    public class BloombaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Flowerbed> Flowerbeds { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Climate> Climates { get; set; }
        public DbSet<FlowerbedCare> FlowerbedCares { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<PlantInFlowerbed> PlantsInFlowerbeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlantInFlowerbed>().HasKey(pif => new { pif.PlantId, pif.FlowerbedId, pif.PlaceId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration;

            // Load configuration from AppSettings.json
            try
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("AppSettings.json")
                    .Build();
            }
            catch (IOException)
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("Bloombase\\AppSettings.json")
                    .Build();
            }

            // Get the connection string
            string connectionString = configuration.GetConnectionString("BloombaseReader");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            // Try to connect to the database
            try
            {
                optionsBuilder.UseSqlServer(builder.ConnectionString);
                using var connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }
        }
    }
}
