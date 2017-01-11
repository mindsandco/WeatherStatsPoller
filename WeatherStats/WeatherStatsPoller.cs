using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EF6CodeFirstTutorials
{
    public class WeatherStatsPoller
    {
        private double temperature;

        private string city;

        private string country;

        public WeatherStatsPoller(string city, string country)
        {
            this.city = city;
            this.country = country;
        }

        public bool IsDone = false;
        
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
                this.IsDone = true;
            }
        }

        public async Task PollDataFromWeb()
        {
            try
            {
        
                //TODO use nuget package and NewtonSoft to parse json properly
                var weburl = GetUrlForCity(city, country);
                var json=  new WebClient().DownloadStringTaskAsync(new Uri(weburl));
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


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GetUrlForCity(string city, string country)
        {
            //Note the API key is unique and cannot be changed.
            return $"http://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid=f20cf3b8e8ea1eb76ca2f9e8c4957bb1";
        }
    }
}
