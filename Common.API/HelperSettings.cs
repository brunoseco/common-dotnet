using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.API
{
    public class HelperSettings
    {

        public static dynamic Get()
        {

            var cns = ConfigurationManager.ConnectionStrings;
            var bancos = new List<string>();
            foreach (var item in cns)
            {
                var banco = item.ToString().Split(';').Where(_ => _.Contains("Catalog"));
                bancos.AddRange(banco);
            }

            return new { setting = string.Join(";", bancos.ToArray()) };

        }



    }
}
