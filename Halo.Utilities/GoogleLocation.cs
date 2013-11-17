using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Halo.Utilities
{
    public class GoogleLocation
    {
        const string apiLatLongFromAddress = "maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false";

        public bool UseHttps { get; set; }

        private string UrlProtocolPrefix
        {
            get
            {
                if (UseHttps)
                {
                    return "https://";
                }
                else
                {
                    return "http://";
                }
            }
        }

        protected string APIUrlLatLongFromAddress
        {
            get
            {
                return UrlProtocolPrefix + apiLatLongFromAddress;
            }
        }

        public MapPoint GetLatLongFromAddress(string address)
        {
            XDocument doc = XDocument.Load(string.Format(APIUrlLatLongFromAddress, address));
            var response = doc.Descendants("result").Descendants("geometry").Descendants("location").First();

            if (null != response)
            {
                var latitude = ParseUS((response.Nodes().First() as XElement).Value);
                var longitude = ParseUS((response.Nodes().ElementAt(1) as XElement).Value);

                return new MapPoint() { Latitude = latitude, Longitude = longitude };
            }

            return null;
        }

        private double ParseUS(string value)
        {
            return Double.Parse(value, new CultureInfo("en-US"));
        }
    }  /// End of Class
}
