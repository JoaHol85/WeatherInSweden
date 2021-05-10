﻿using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherInSweden.Models;

namespace WeatherInSweden.Services
{
    public interface IDAL
    {
        public MongoClient GetMongoClient();
        public Task Save25DaysBackOfWeatherDataForCity(string city);
        public IMongoCollection<DailyWeather> DailyWeatherCollection();
        public IEnumerable<DailyWeather> GetDailyWeatherDataForCity(string city);


    }
}