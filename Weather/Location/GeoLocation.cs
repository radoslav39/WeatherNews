using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.Location
{
    public class GeoLocation
    {
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}