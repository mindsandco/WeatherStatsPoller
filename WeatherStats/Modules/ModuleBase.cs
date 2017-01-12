using System;
using System.Data.SqlClient;
using System.Threading;
using WeatherStats;

namespace Modules
{
    public abstract class ModuleBase :IDisposable
    {
        public ModuleBase(string name)
        {
            this.ModuleName = name;
            var hostName = System.Configuration.ConfigurationManager.AppSettings["DatabaseHostName"];
            var databaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
            var userId = System.Configuration.ConfigurationManager.AppSettings["userId"];
            var password = System.Configuration.ConfigurationManager.AppSettings["password"];

            //TODO add some check that the values are populated in the config file

            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = hostName;
            builder.InitialCatalog = databaseName;
            builder.UserID = userId;
            builder.Password = password;
            builder.IntegratedSecurity = false;
            this.Database = new Database(builder.ConnectionString);
        }

        public Database Database;
        private Thread workThread;
        public bool StartModule()
        {
            this.closingDown = false;
            this.workThread = new Thread(this.DoWork);
            this.workThread.Start();

            var ticks = Environment.TickCount;
            while (ticks + 3000 > Environment.TickCount && !this.workThread.IsAlive)
            {
                Thread.Sleep(10);
            }
            return this.workThread.IsAlive;
        }


        public bool StopModule()
        {
            this.closingDown = true;
            var ticks = Environment.TickCount;
            while (ticks + 60000 > Environment.TickCount && !this.RanToEnd)
            {
                Thread.Sleep(10);
            }
            this.workThread.Join();
            return this.RanToEnd;

        }

        private bool closingDown;

        protected bool RanToEnd { get; set; }

        protected bool ClosingDown => this.closingDown;

        // TODO refactor this to use async/await properly
        protected abstract void DoWork();



        public string ModuleName;

        public void Dispose()
        {
            if (this.Database != null)
            {
                this.Database.Dispose();
                this.Database = null;
            }
        }
    }
}
