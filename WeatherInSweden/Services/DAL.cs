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
using WeatherInSweden.Pages;

namespace WeatherInSweden.Services
{
    public class DAL : IDAL
    {
        private readonly HttpClient _httpClient;

        public DAL(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public MongoClient GetMongoClient()
        {
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build().GetConnectionString("MongoConnectionString");
            MongoClient client = new(connectionString);
            return client;
        }

        // FUNGERAR SOM DEN SKA //
        public async Task Save25DaysBackOfWeatherDataForCityAsync(string city)           
        {
            List<DailyWeather> listOfdailyWeather = GetDailyWeatherDataForCity(city).ToList();
            for (int i = 0; i < 25; i++) 
            {
                string date1 = DateTime.Now.AddDays(i - 26).Date.ToString().Substring(0, 10);
                string date2 = DateTime.Now.AddDays(i -25).Date.ToString().Substring(0, 10);

                var apiClient = _httpClient;
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
                        RelativeHumidity = dailyWeather.data[0].rh,
                        Temperature = dailyWeather.data[0].temp,
                        WindDirection = dailyWeather.data[0].wind_dir,
                        WindSpeed = dailyWeather.data[0].wind_spd
                    }
                };

                bool excistInDB = listOfdailyWeather.Any(q => q.Date == weather.Date);
                if(!excistInDB)
                {
                    await DailyWeatherCollection().InsertOneAsync(weather);
                }
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
            return collection.Find(new BsonDocument()).ToList().Where(q => q.City == city).OrderBy(q => q.Date);
        }


        public CityWeatherInfo GetWeatherForCity(string city)
        {
            CityWeatherInfo weather = new();
            var collectedWeatherInCity = GetDailyWeatherDataForCity(city).ToList();
            weather.City = city;

            Values avgValues = new();
            avgValues.MaxUV = Math.Round(collectedWeatherInCity.Average(q => q.Values.MaxUV), 2);
            avgValues.RelativeHumidity = Math.Round(collectedWeatherInCity.Average(q => q.Values.RelativeHumidity), 2);
            avgValues.Temperature = Math.Round(collectedWeatherInCity.Average(q => q.Values.Temperature), 2);
            avgValues.Clouds = Math.Round(collectedWeatherInCity.Average(q => q.Values.Clouds), 2);
            avgValues.WindDirection = Math.Round(collectedWeatherInCity.Average(q => q.Values.WindDirection), 2);
            avgValues.WindSpeed = Math.Round(collectedWeatherInCity.Average(q => q.Values.WindSpeed), 2);
            weather.AverageValues = avgValues;

            Values minValues = new();
            minValues.MaxUV = Math.Round(collectedWeatherInCity.Min(q => q.Values.MaxUV), 2);
            minValues.RelativeHumidity = Math.Round(collectedWeatherInCity.Min(q => q.Values.RelativeHumidity), 2);
            minValues.Temperature = Math.Round(collectedWeatherInCity.Min(q => q.Values.Temperature), 2);
            minValues.Clouds = Math.Round(collectedWeatherInCity.Min(q => q.Values.Clouds), 2);
            minValues.WindDirection = Math.Round(collectedWeatherInCity.Min(q => q.Values.WindDirection), 2);
            minValues.WindSpeed = Math.Round(collectedWeatherInCity.Min(q => q.Values.WindSpeed), 2);
            weather.MinValues = minValues;

            Values maxValues = new();
            maxValues.MaxUV = Math.Round(collectedWeatherInCity.Max(q => q.Values.MaxUV), 2);
            maxValues.RelativeHumidity = Math.Round(collectedWeatherInCity.Max(q => q.Values.RelativeHumidity), 2);
            maxValues.Temperature = Math.Round(collectedWeatherInCity.Max(q => q.Values.Temperature), 2);
            maxValues.Clouds = Math.Round(collectedWeatherInCity.Max(q => q.Values.Clouds), 2);
            maxValues.WindDirection = Math.Round(collectedWeatherInCity.Max(q => q.Values.WindDirection), 2);
            maxValues.WindSpeed = Math.Round(collectedWeatherInCity.Max(q => q.Values.WindSpeed), 2);
            weather.MaxValues = maxValues;

            return weather;
        }

        public MeteorologicalDates GetMeteorologicalDates(string city)
        {
            MeteorologicalDates dates = new();

            var collectedWeatherInCity = GetDailyWeatherDataForCity(city).OrderBy(c => c.Date).ToList();
            dates.MeteorologicalSummer = MeteorologicalSummer(collectedWeatherInCity);
            dates.MeteorolgicalSpring = MeteorologicalSpring(collectedWeatherInCity);
            return dates;
        }

        //Sommar 5 dygn i rad över 10
        public string MeteorologicalSummer(List<DailyWeather> weatherList)
        {
            string startDate = "";
            int daysInRow = 0;

            for (int i = 0; i < weatherList.Count(); i++)
            {
                daysInRow = 0;
                startDate = weatherList[i].Date;

                for (int y = 1; y < weatherList.Count(); y++)
                {
                    if (daysInRow >= 5)
                    {
                        return startDate;
                    }
                    if (weatherList[y].Values.Temperature >= 10)
                    {
                        daysInRow++;
                        continue;
                    }
                    if (weatherList[y].Values.Temperature < 10)
                    {
                        break;
                    }
                }
            }
            return "";
        }

        public string MeteorologicalSpring(List<DailyWeather> weatherList)
        {
            string startDate = "";
            int daysInRow = 0;

            for (int i = 0; i < weatherList.Count(); i++)
            {
                daysInRow = 0;
                startDate = weatherList[i].Date;

                for (int y = 1; y < weatherList.Count(); y++)
                {
                    if (daysInRow >= 7)
                    {
                        return startDate;
                    }
                    if (weatherList[y].Values.Temperature > 0)
                    {
                        daysInRow++;
                        continue;
                    }
                    if (weatherList[y].Values.Temperature <= 0)
                    {
                        break;
                    }
                }
            }
            return "";
        }
    }
}
