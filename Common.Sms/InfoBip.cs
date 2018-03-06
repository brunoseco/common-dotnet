using Common.Domain.Enums;
using Common.Domain.Interfaces;
using System.Collections.Generic;

namespace Common.Sms
{
    public class InfoBip : ISMS
    {
        private Dictionary<string, string> mensagens;

        public InfoBip()
        {
            this.mensagens = new Dictionary<string, string>();
        }

        public void Reset()
        {
            this.mensagens = new Dictionary<string, string>();
        }

        public string User { get; set; }
        public string Password { get; set; }
        public TipoSMS Tipo { get; set; }
        public string PhoneNumberFrom { get; set; }

        public void Add(string phoneNumber, string message)
        {
            if (message.Length < 4096)
                this.mensagens.Add(phoneNumber, message);
        }

        public RetornoSMS Send()
        {
            /*
            var client = new RestClient("https://api.infobip.com/sms/1/text/single");

            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
            request.AddParameter("application/json", "{\"from\":\"InfoSMS\", \"to\":[  \"41793026727\",\"41793026834\"],\"text\":\"Test SMS.\"}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            
            if (retorno.StartsWith("OK"))
                return RetornoSMS.OK;
            else
                return RetornoSMS.Erro;
            */

            return RetornoSMS.OK;
        }
    }
}
