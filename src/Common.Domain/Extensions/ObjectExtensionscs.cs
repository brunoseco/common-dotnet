using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public static class ObjectExtensionscs
    {

        public static decimal MeasureItInMB(this object model)
        {
            var serialization = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            decimal bytesCout = System.Text.Encoding.UTF8.GetByteCount(serialization);
            var length = (Math.Round((bytesCout / 1024) / 1024, 4));
            return length;
        }

        public static decimal MeasureItInMB(this string serialization)
        {
            decimal bytesCout = System.Text.Encoding.UTF8.GetByteCount(serialization);
            var length = (Math.Round((bytesCout / 1024) / 1024, 4));
            return length;
        }

    }
}
