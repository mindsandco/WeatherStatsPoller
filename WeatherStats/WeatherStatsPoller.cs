using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace WeatherStats
{
    public class WeatherStatsPoller
    {
        //TODO poll other values, such as min and max temp, huidity etc.
        private double temperature;

        private string city;

        private string country;

        public WeatherStatsPoller(string city, string country)
        {
            this.city = city;
            this.country = country;
        }

        public string City => this.city;
        
        public string Country => this.country;

        public double Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
            }
        }

        public async Task<double> PollDataFromWeb()
        {
            try
            {
                //TODO use nuget package and NewtonSoft to parse json properly
                var weburl = GetUrlForCity(city, country);
                

                var json = Task.Factory.StartNew<string>(() => new WebClient().DownloadStringTaskAsync(new Uri(weburl)).Result);

                await json;
                var splitToString = json.Result.Split(':');
                for(int i = 0 ; i!= splitToString.Length;i++)
                {
                    if (splitToString[i].Contains("temp") && i < splitToString.Length)
                    {
                        var subStr = splitToString[i + 1].Split(',')[0];
                        double temp = double.Parse(subStr.Replace('.',',')) - 273.15;
                        Temperature = temp;
                        break;
                    }
                }

                return Temperature;


            }
            catch (Exception e)
            {
                Debug.Assert(false,e.Message);
            }
            return -1000;
        }

        private string GetUrlForCity(string city, string country)
        {
            //Note the API key is unique and cannot be changed.
            return $"http://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid=f20cf3b8e8ea1eb76ca2f9e8c4957bb1";
        }
    }
}
