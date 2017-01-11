using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using EF6CodeFirstTutorials;
using WeatherStats.Model;

namespace WeatherStats
{
    class Program
    {
        

        static void Main(string[] args)
        {
            
            Console.WriteLine("*** Starting Weather Stats data acquisition ***");
            Console.WriteLine("");

            var hostName =  System.Configuration.ConfigurationManager.AppSettings["DatabaseHostName"];
            var databaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
            var userId = System.Configuration.ConfigurationManager.AppSettings["userId"];
            var password= System.Configuration.ConfigurationManager.AppSettings["password"];

            //TODO add some check that the values are populated in the config file

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = hostName;
            builder.InitialCatalog = databaseName;
            builder.UserID = userId;
            builder.Password = password;
            builder.IntegratedSecurity = false;
            using (var context = new WeatherStatsContext(builder.ConnectionString))
            
            {
                context.Students.Add(new Student() { StudentName = "New Student using sp" });
                context.SaveChanges();
            }

            WeatherStatsPoller wsp = new WeatherStatsPoller("Aarhus","DK");
            Task.Run(()=>wsp.PollDataFromWeb().ConfigureAwait(false));

            while (wsp.IsDone == false)
            {
                
            }

            Console.WriteLine($"I think the temperature in {wsp.City},{wsp.Country} is {wsp.Temperature} C");
            Console.WriteLine("*** Done with Weather Stats data acquisition ***");
        }
    }
}



