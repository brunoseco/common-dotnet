using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common.Government
{
    public class IGPM
    {
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorProRata { get; set; }

        public IEnumerable<IGPM> Get(DateTime start, DateTime end)
        {
            var binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            var ep = new EndpointAddress("https://www3.bcb.gov.br/wssgs/services/FachadaWSSGS");

            var factory = new ChannelFactory<Igpm.FachadaWSSGS>(binding, ep);
            factory.Open();
            var channel = factory.CreateChannel();

            var xml = channel.getValoresSeriesXML(new Igpm.getValoresSeriesXMLRequest
            {
                in0 = new long[] { 189 },
                in1 = start.Date.ToShortDateString(),
                in2 = end.Date.ToShortDateString()
            });

            var document = XDocument.Parse(xml.getValoresSeriesXMLReturn);

            var result = from dados in document.Descendants("SERIE").Elements("ITEM")
                         where (bool)dados.Element("BLOQUEADO") == false
                         select new
                         {
                             Valor = (decimal)dados.Element("VALOR"),
                             ValorProRata = proRata((decimal)dados.Element("VALOR"), ((DateTime)dados.Element("DATA")).MonthDays()),
                             Data = (DateTime)dados.Element("DATA")
                         };

            return result.Select(_ => new IGPM
            {
                Data = _.Data,
                Valor = _.Valor,
                ValorProRata = _.ValorProRata
            });
        }

        private decimal proRata(decimal indice, int days)
        {
            var exp = (+indice / 100) + 1;
            var exp2 = (1 / (decimal)days);
            var result = (Math.Pow(Convert.ToDouble(exp), Convert.ToDouble(exp2)) - 1) * 100;
            return (decimal)result;

            //(((+B4/100)+1)^(1/G4)-1)*100
        }



    }
}
