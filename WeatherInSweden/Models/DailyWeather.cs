using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherInSweden.Models
{
    public class DailyWeather
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public Values Values { get; set; }
    }

    //public class Values
    //{
    //    public double Temperature { get; set; }
    //    public double WindSpeed { get; set; }
    //    public double RelativeHumidity { get; set; }
    //    public double Clouds { get; set; }
    //    public double WindDirection { get; set; }
    //    public double MaxUV { get; set; }
    //}
}
