using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Modules;
using WeatherStats.Model;

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
                WeatherStatsPoller wsp = new WeatherStatsPoller(this.City, this.Country);
                while (false == this.ClosingDown)
                {
                    var msToSleep = Math.Abs((DateTime.Now - DateTime.Now.ToTenMinutePrecision()).TotalMilliseconds);
                    Thread.Sleep(Convert.ToInt32(msToSleep));
                    var measuredDoubleValue = await wsp.PollDataFromWeb();
                    var measurement = new WeatherMeasurement(DateTime.Now, measuredDoubleValue);
                    try
                    {

                        this.Database.WeatherMeasurement.Add(measurement);
                        this.Database.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        // database is not avilable, store the data in a file
                        var ser = new BinaryFormatter();
                        using (var memStream = new MemoryStream())
                        {
                            ser.Serialize(memStream, measurement);
                            var array =memStream.ToArray();
                            var path= System.Configuration.ConfigurationManager.AppSettings["OfflineStoragePath"];
                            if (Directory.Exists(path) == false)
                            {
                                Directory.CreateDirectory(path);
                            }
                            File.WriteAllBytes($"{path}\\{measurement.Timestamp.Timestamp2String(true,true)}",array);
                        }


                    }
                    
                }

                this.RanToEnd = true;


            }
            catch (ThreadAbortException)
            {
                // here we end up if the module is being closed down
                int i = 34;
            }
            catch (Exception e)
            {
                Debug.Assert(false,e.Message);
            }
            
        }
    }
}
