using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Weather.Location
{
    public class GeoLocator : IDisposable
    {
        private static HttpClient client = new HttpClient();
        
        public async Task<GeoLocation> Locate(string ip)
        {
            var response = await client.GetAsync($"http://freegeoip.net/json/{ip}");
            var content = await response.Content.ReadAsStringAsync();

            var prototype = new
            {
                country_code = "",
                city = "",
                zip_code = "",
                latitude = 0d,
                longitude = 0d
            };

            var location = JsonConvert.DeserializeAnonymousType(content, prototype);
            var geoLocation = new GeoLocation()
            {
                CountryCode = location.country_code,
                City = location.city,
                ZipCode = location.zip_code,
                Latitude = location.latitude,
                Longitude = location.longitude
            };

            return geoLocation;
        }

        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
    }
}