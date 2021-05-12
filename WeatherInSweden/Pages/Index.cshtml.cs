using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherInSweden.Services;

namespace WeatherInSweden.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDAL _IDAL;
        public IndexModel(IDAL iDAL)
        {
            _IDAL = iDAL;
        }

        public string[] Cities { get; set; } = { "Kiruna", "Sundsvall", "Stockholm", "Jönköping", "Göteborg", "Malmö" };
        [BindProperty]
        public string City { get; set; }

        public void OnGet()
        {
        }

        public void OnPostSaveWeatherData()
        {
            _IDAL.Save25DaysBackOfWeatherDataForCity(City);
            OnGet();
        }

        public void OnPostShowWeatherDataForCity()
        {
            _IDAL.GetDailyWeatherDataForCity(City);
            OnGet();
        }



    }
}
