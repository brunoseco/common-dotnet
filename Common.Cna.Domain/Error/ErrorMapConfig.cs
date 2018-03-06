using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Error
{
    public class ErrorMapConfig : Dictionary<string, string>
    {
        private static ErrorMapConfig _instance;
        private ErrorMapConfig()
        {
            this.Add("Financeiro já Baixado", "Financeiro já Baixado");
            this.Add("too many connections", "Número de envios muito alto, aguarde alguns instantes.");
            this.Add("limite de consumo", "Número de envios muito alto, aguarde alguns instantes.");
            _instance = this;
        }

        public static ErrorMapConfig GetConfig()
        {
            if (_instance.IsNotNull())
                return _instance;

            return new ErrorMapConfig();

        }
    }
}
