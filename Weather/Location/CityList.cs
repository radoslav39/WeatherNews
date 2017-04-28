using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.Location
{
    public class CityList
    {
        private List<City> cities;

        private List<City> Create()
        {
            var prototype = new[] {
                new
                {
                    id = 0,
                    name = "",
                    country = "",
                    coord = new
                    {
                        lon = 0d,
                        lat = 0d
                    }
                }
            };

            var cities = JsonConvert.DeserializeAnonymousType(Weather.Cities.city_list, prototype);
            var result = cities.Select(x => new City()
            {
                Id = x.id,
                Name = x.name,
                Country = x.country,
                Coord = new Location.Coord
                {
                    Latitude = x.coord.lat,
                    Longitude = x.coord.lon
                }
            }).ToList();

            return result;
        }

        public List<City> GetCities()
        {
            if (cities == null)
            {
                cities = Create();
            }

            return cities;
        }
    }
}