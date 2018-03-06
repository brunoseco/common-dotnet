using Common.API;
using Common.Domain.Geolocalization;
using Common.Domain.Interfaces;
using System.Configuration;

namespace Common.Geolocalization
{
    public class GoogleMaps : IGeolocalization
    {
        public string Key { get; set; }
        public bool TryAgain { get; set; }

        public GoogleMaps()
        {
            this.Key = ConfigurationManager.AppSettings["KeyGoogleMaps"];
            this.TryAgain = true;
        }

        public Localization FindByAddress(string address)
        {
            var _request = new HelperHttp("https://maps.googleapis.com");

            var _params = new QueryStringParameter();

            _params.Add("address", address);
            _params.Add("region", "BR");
            _params.Add("key", this.Key);

            var _response = _request.GetBasic<dynamic>("/maps/api/geocode/json", _params);
            if (_response.status == "OK")
            {
                var _results = _response.results;
                if (_results != null)
                {
                    var _geometry = _results[0].geometry;
                    if (_geometry != null)
                    {
                        var _location = _geometry.location;
                        if (_location != null)
                            return new Localization() { Latitude = _location.lat, Longitude = _location.lng };
                    }
                }

                return null;
            }
            else if (_response.status == "OVER_QUERY_LIMIT")
            {
                if (this.TryAgain)
                {
                    this.TryAgain = false;
                    return this.FindByAddress(address);
                }
            }

            return null;
        }
    }
}
