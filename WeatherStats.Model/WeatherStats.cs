using System.Data.Entity;

namespace WeatherStats.Model
{
    class WeatherStatsContext : DbContext
    {
        public WeatherStatsContext(string connectionString)
            : base(connectionString)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Standard> Standards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Custom conventions demo
            //Make an _ID property as PrimaryKey eg. Student_ID, Standard_ID and so
            modelBuilder
                .Properties()
                .Where(p => p.Name == p.DeclaringType.Name + "_ID")
                .Configure(p => p.IsKey());

            //Configure RowVersion property as Concurrency token
            modelBuilder.Properties()
                .Where(p => p.Name == "RowVersion")
                .Configure(p => p.IsConcurrencyToken(true));

            //Configure Description column to nvarchar(200)
            modelBuilder
                .Properties()
                .Where(p => p.Name == "Description")
                .Configure(p => p.HasMaxLength(200));
            ////Create Stored Procedures to Add/Update/Delete Student entity on SaveChanges 
            modelBuilder.Entity<Student>()
                .MapToStoredProcedures();

            //Customize SP name, param name & result param name
            modelBuilder.Entity<Student>()
                .MapToStoredProcedures(p => p.Insert(sp => sp.HasName("sp_InsertStudent").Parameter(pm => pm.StudentName, "name").Result(rs => rs.Student_ID, "Student_ID"))
                .Update(sp => sp.HasName("sp_UpdateStudent").Parameter(pm => pm.StudentName, "name"))
                .Delete(sp => sp.HasName("sp_DeleteStudent").Parameter(pm => pm.Student_ID, "Id"))
                );

            //Configure all entities to use default stored procedures for CUD operation
            //modelBuilder.Types().Configure(t => t.MapToStoredProcedures());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
