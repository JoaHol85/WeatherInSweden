using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherInSweden.Models
{
    public class WeatherData
    {
        //public string timezone { get; set; }
        //public string state_code { get; set; }
        //public string country_code { get; set; }
        //public float lat { get; set; }
        //public float lon { get; set; }
        public string city_name { get; set; }
        //public string station_id { get; set; }
        public Datum[] data { get; set; }
        //public string[] sources { get; set; }
        //public string city_id { get; set; }
    }

    public class Datum
    {
        public float rh { get; set; }
        //public int max_wind_spd_ts { get; set; }
        //public float t_ghi { get; set; }
        //public float max_wind_spd { get; set; }
        //public float solar_rad { get; set; }
        //public float wind_gust_spd { get; set; }
        //public int max_temp_ts { get; set; }
        //public int min_temp_ts { get; set; }
        public int clouds { get; set; }
        //public int max_dni { get; set; }
        //public float precip_gpm { get; set; }
        public float wind_spd { get; set; }
        //public float slp { get; set; }
        //public int ts { get; set; }
        //public float max_ghi { get; set; }
        public float temp { get; set; }
        //public float pres { get; set; }
        ////public int dni { get; set; }
        //public float dewpt { get; set; }
        //public int snow { get; set; }
        //public float dhi { get; set; }
        //public int precip { get; set; }
        public int wind_dir { get; set; }
        ////public int max_dhi { get; set; }
        //public float ghi { get; set; }
        //public int max_temp { get; set; }
        //public float t_dni { get; set; }
        public float max_uv { get; set; }
        //public float t_dhi { get; set; }
        public string datetime { get; set; }
        //public float t_solar_rad { get; set; }
        //public float min_temp { get; set; }
        //public int max_wind_dir { get; set; }
        //public object snow_depth { get; set; }
    }
}
