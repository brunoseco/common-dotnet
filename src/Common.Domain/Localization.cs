using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Geolocalization
{
    [Serializable]
    public class Localization
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    }
}
