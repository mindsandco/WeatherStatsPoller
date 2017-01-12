using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Modules;
using WeatherStats.Shared;

namespace WeatherStats.Modules
{
    public class WeatherPollerModule : ModuleBase
    {

        public WeatherPollerModule(string country, string city) : base(nameof(WeatherPollerModule))
        {
            this.Country = country;
            this.City = city;
        }

        public string Country;

        public string City;

        protected override async void DoWork()
        {
            try
            {
                var wsp = new WeatherStatsPoller(this.City, this.Country);
                while (false == this.ClosingDown)
                {
                    var now = DateTime.Now;
                    var pollTime10MinPrecision = now.ToTenMinutePrecision();
                    var ts = new TimeSpan(0,0,10,0)- (now - pollTime10MinPrecision);
                    Thread.Sleep(Convert.ToInt32(ts.TotalMilliseconds));
                    var measuredDoubleValue = await wsp.PollDataFromWeb();
                    var measurement = new WeatherMeasurement(pollTime10MinPrecision, measuredDoubleValue);
                    try
                    {
                        this.Database.WeatherMeasurement.Add(measurement);
                        this.Database.SaveChanges();
                        Console.WriteLine($"Web says that in {wsp.Country},{wsp.City} the temperature is {measurement.TemperatureMeasurement} measured at: {measurement.Timestamp.Timestamp2String(false)}");
                    }
                    catch (Exception e)
                    {
                        // database is not avilable, store the data in a file
                        StoreMeasurementToOfflineStorage(measurement);
                    }
                    
                }

                this.RanToEnd = true;


            }
            catch (ThreadAbortException)
            {
                // if the thead is being aborted
            }
            catch (Exception e)
            {
                Debug.Assert(false,e.Message);
            }
            
        }

        private void StoreMeasurementToOfflineStorage(WeatherMeasurement measurement)
        {
            var ser = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                ser.Serialize(memStream, measurement);
                var array = memStream.ToArray();
                var path = System.Configuration.ConfigurationManager.AppSettings["OfflineStoragePath"];
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes($"{path}\\{measurement.Timestamp.Timestamp2String(true, true)}", array);
            }
        }

        private WeatherMeasurement ReadMeasurementFromFile(string file)
        {
            //TODO implement this so we can migrate the offline files into the database
            return new WeatherMeasurement(DateTime.MinValue, 0);
        }
    }
}
