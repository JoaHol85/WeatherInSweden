using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherInSweden.Models
{
    public class Values
    {
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double RelativeHumidity { get; set; }
        public double Clouds { get; set; }
        public double WindDirection { get; set; }
        public double MaxUV { get; set; }
    }
}
