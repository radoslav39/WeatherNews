using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Weather.Location;
using Weather.ViewModels;

namespace Weather.Controllers
{
    public class WeatherController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly GeoLocator geoLocator = new GeoLocator();
        private readonly CityList cityList = new CityList();

        [HttpGet]
        public async Task<ActionResult> Widget()
        {
            var localAddresses = new HashSet<string>() { "localhost", "127.0.0.1", "::1" };
            var requestAddress = Request.UserHostAddress;

            if (localAddresses.Contains(requestAddress))
            {
                var response = await client.GetAsync("http://api.ipify.org/?format=json");
                var content = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeAnonymousType(content, new { ip = "" });
                requestAddress = json.ip;
            }

            var geoLocation = await geoLocator.Locate(requestAddress);
            var cities = cityList.GetCities();
            var city = cities.SingleOrDefault(x => x.Name.Equals(geoLocation.City, StringComparison.OrdinalIgnoreCase)
                && x.Country.Equals(geoLocation.CountryCode, StringComparison.OrdinalIgnoreCase)
                || (x.Coord != null && Math.Abs(x.Coord.Latitude - geoLocation.Latitude) < 0.01 && Math.Abs(x.Coord.Longitude - geoLocation.Longitude) < 0.01));

            if (city == null)
            {
                return HttpNotFound();
            }

            return PartialView("_WeatherWidget", new WeatherWidgetModel { CityId = city.Id });
        }
    }
}