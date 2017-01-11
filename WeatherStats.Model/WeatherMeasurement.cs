using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherStats.Model
{
    [Serializable]
    public class WeatherMeasurement
    {

        //TODO expand the model to inclue a "measurement source" that enables us to have different sources for WeatherMeasurements data
        // It should be possible to configure how often data is polled and from where
        public WeatherMeasurement(DateTime timestamp, double measuredValue)
        {
            this.TemperatureMeasurement = measuredValue;
            this.Timestamp = timestamp;
        }

        public int WeatherMeasurement_ID { get; set; }

        public double TemperatureMeasurement { get; set; }


        public DateTime Timestamp { get; set; }
    }
}
