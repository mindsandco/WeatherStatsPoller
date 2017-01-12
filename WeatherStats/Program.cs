using System;
using System.Data.SqlClient;
using System.Threading;
using WeatherStats.Modules;

namespace WeatherStats
{
    class Program
    {
        

        static void Main(string[] args)
        {
            
            Console.WriteLine("*** Starting Weather Stats data acquisition ***");
            Console.WriteLine("");
            ModuleManager moduleManager = new ModuleManager();
            moduleManager.AddModule(new WeatherPollerModule("DK","Aarhus"));

            //TODO Make a module that scans the offline files folder and importes these
            // TODO Make a module or similar that checks for missing data and creates the values by interpolating the existing data
            moduleManager.StartAllModules();
            Console.WriteLine("Press Q to quit");
            while (moduleManager.KeepRunnning)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    moduleManager.StopAllModules();
                }
            }

            


            Console.WriteLine("*** Done with Weather Stats data acquisition ***");
        }
    }
}



