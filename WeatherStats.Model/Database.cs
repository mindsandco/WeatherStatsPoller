using System.Data.Entity;
using WeatherStats.Model;

namespace WeatherStats
{
    public class Database : DbContext
    {
        public Database(string connectionString)
            : base(connectionString)
        {

        }
        
        public DbSet<WeatherMeasurement> WeatherMeasurement { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Make an _ID property as PrimaryKey eg. Student_ID, Standard_ID and so
            modelBuilder
                .Properties()
                .Where(p => p.Name == p.DeclaringType.Name + "_ID")
                .Configure(p => p.IsKey());
            base.OnModelCreating(modelBuilder);
        }
    }
}
