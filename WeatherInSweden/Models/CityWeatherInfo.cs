using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherInSweden.Models
{
    public class CityWeatherInfo
    {
        public string City { get; set; }
        public Values MinValues { get; set; }
        public Values AverageValues { get; set; }
        public Values MaxValues { get; set; }
    }
}
