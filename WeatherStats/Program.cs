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
            //TODO Make a module that exposes the weather data as a webservice using a timestamp as index for retrieving single measurements
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



