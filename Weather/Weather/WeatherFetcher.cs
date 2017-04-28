//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web;
//using Weather.Location;

//namespace Weather.Weather
//{
//    public class WeatherFetcher : IDisposable
//    {
//        private static HttpClient client = new HttpClient();

//        public async Task<GeoLocation> GetWeatherFor(GeoLocation geoLocation)
//        {
//            HttpResponseMessage response;

//            if (geoLocation.Latitude != 0 || geoLocation.Latitude != 0)
//            {
//                response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?lat={geoLocation.Latitude}&lon={geoLocation.Longitude}");
//            }
//            else if (!string.IsNullOrEmpty(geoLocation.ZipCode))
//            {
//                var query = !string.IsNullOrEmpty(geoLocation.CountryCode)
//                    ? $"{geoLocation.ZipCode},{geoLocation.CountryCode}"
//                    : $"{geoLocation.ZipCode}";

//                response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?zip={query}");
//            }
//            else if (!string.IsNullOrEmpty(geoLocation.City))
//            {
//                var query = !string.IsNullOrEmpty(geoLocation.CountryCode)
//                    ? $"{geoLocation.City},{geoLocation.CountryCode}"
//                    : $"{geoLocation.City}";

//                response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={query}");
//            }
//            else
//            {
//                throw new ArgumentException("Geo Location info is insufficient for a weather request!");
//            }

//            var content = await response.Content.ReadAsStringAsync();

//            var prototype = new
//            {
//                country_code = "",
//                city = "",
//                zip_code = "",
//                latitude = 0d,
//                longitude = 0d
//            };

//            var location = JsonConvert.DeserializeAnonymousType(content, prototype);
//        }

//        public void Dispose()
//        {
//            if (client != null)
//            {
//                client.Dispose();
//                client = null;
//            }
//        }
//    }
//}