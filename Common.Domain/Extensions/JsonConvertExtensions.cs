using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Extensions
{
    public static class JsonConvertExtensions
    {

        public static string SerializeObjectWithIgnore(this object value)
        {

            return JsonConvert.SerializeObject(value,
            Newtonsoft.Json.Formatting.None,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });

        }


    }
}
