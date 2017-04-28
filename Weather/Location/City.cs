using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.Location
{
    public class Coord
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public Coord Coord { get; set; }
    }
}