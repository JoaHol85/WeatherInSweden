using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherInSweden.Models;
using System.Text.Json;
using MongoDB.Bson;

namespace WeatherInSweden.Services
{
    public class DAL : IDAL
    {
        //private readonly IConfiguration _configuration;
        //public DAL(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public MongoClient GetMongoClient()
        {
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build().GetConnectionString("MongoConnectionString");
            MongoClient client = new(connectionString);
            return client;
        }

        // FUNGERAR SOM DEN SKA //
        public async Task Save25DaysBackOfWeatherDataForCity(string city)           
        {
            for (int i = 0; i < 3; i++) 
            {
                string date1 = DateTime.Now.AddDays(i - 26).Date.ToString().Substring(0, 10);
                string date2 = DateTime.Now.AddDays(i -25).Date.ToString().Substring(0, 10);

                var apiClient = new HttpClient();
                var collection = new DAL();
                Task<string> getWeatherString = apiClient.GetStringAsync($"https://api.weatherbit.io/v2.0/history/daily?city={city}&start_date={date1}&end_date={date2}&key=9b13ed2386354ee08795a65c6caf789f");
                string weatherString = await getWeatherString;
                var dailyWeather = JsonSerializer.Deserialize<WeatherData>(weatherString);

                DailyWeather weather = new()
                {
                    Id = Guid.NewGuid(),
                    City = dailyWeather.city_name,
                    Date = dailyWeather.data[0].datetime,
                    Values = new()
                    {
                        Clouds = dailyWeather.data[0].clouds,
                        MaxUV = dailyWeather.data[0].max_uv,
                        RelativeHumidity = Math.Round(dailyWeather.data[0].rh, 2),
                        Temperature = Math.Round(dailyWeather.data[0].temp, 2),
                        WindDirection = dailyWeather.data[0].wind_dir,
                        WindSpeed = Math.Round(dailyWeather.data[0].wind_spd, 2)
                    }
                };

                await collection.DailyWeatherCollection().InsertOneAsync(weather);
            }
        }

        public IMongoCollection<DailyWeather> DailyWeatherCollection()
        {
            var client = GetMongoClient();
            var mongoDB = client.GetDatabase("WeatherDB");
            var weatherCollection = mongoDB.GetCollection<DailyWeather>("WeatherCollection");
            return weatherCollection;
        }

        public IEnumerable<DailyWeather> GetDailyWeatherDataForCity(string city)
        {
            var client = GetMongoClient();
            var MongoDB = client.GetDatabase("WeatherDB");
            var collection = MongoDB.GetCollection<DailyWeather>("WeatherCollection");
            return collection.Find(new BsonDocument()).ToList().Where(q => q.City == city);
        }

    }
}
