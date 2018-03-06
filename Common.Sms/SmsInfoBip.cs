using Common.API;
using Common.Domain.Enums;
using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sms
{
    public class SmsInfoBip : ISMS
    {

        #region Types from integration
        private class Destination
        {
            public string from { get; set; }
            public string to { get; set; }
            public string text { get; set; }
        }
        private class RequestData
        {
            public List<Destination> messages { get; set; }
        }
        private class ResponseData
        {
            public string bulkId { get; set; }
            public Message[] messages { get; set; }

        }
        private class Message
        {
            public string To { get; set; }
            public int smsCount { get; set; }
            public string messageId { get; set; }
            public Status status { get; set; }

        }
        private class Status
        {
            public int groupId { get; set; }
            public string groupName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }
        #endregion

        private string endPointApiInfoBip;

        private RequestData pendingSms;

        public SmsInfoBip()
        {
            this.endPointApiInfoBip = ConfigurationManager.AppSettings["endPointApiSMS"];
            this.pendingSms = new RequestData
            {
                messages = new List<Destination>()
            };
        }
        public void Reset()
        {
            if (this.pendingSms.IsNotNull() && this.pendingSms.messages.IsAny()) this.pendingSms.messages.Clear();
        }
        public string User { get; set; }
        public string Password { get; set; }
        public TipoSMS Tipo { get; set; }
        public string PhoneNumberFrom { get; set; }

        public void Add(string phoneNumber, string content)
        {
            this.pendingSms.messages.Add(new Destination
            {
                from = this.PhoneNumberFrom,
                to = FixFormatPhoneNumber(phoneNumber),
                text = content
            });
        }

        private static string FixFormatPhoneNumber(string phoneNumber)
        {
            phoneNumber = ClearPhoneNumber(phoneNumber);

            if (phoneNumber.Length <= 11)
                return string.Format("55{0}", phoneNumber);
            return phoneNumber;
        }

        private static string ClearPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "")
                .Replace(" ", "");

            return phoneNumber;
        }

        public Domain.Enums.RetornoSMS Send()
        {
            var request = new HelperHttp(endPointApiInfoBip);
            request.AddCustomHeaders("authorization", BasicAuth.Base64Encoding(string.Format("{0}:{1}", this.User, this.Password)));
            var response = request.PostBasic<RequestData, ResponseData>("/sms/1/text/multi", this.pendingSms);

            if (response.IsNull())
                return RetornoSMS.Erro;

            try
            {
                if (response.messages.NotIsAny())
                    return RetornoSMS.Erro;

                var notAccept = response.messages
               .Where(_ => _.status.groupName != "ACCEPTED")
               .Where(_ => _.status.groupName != "PENDING")
               .Where(_ => _.status.groupName != "DELIVERED")
               .Any();

                if (notAccept)
                    return RetornoSMS.Erro;

                return RetornoSMS.OK;


            }
            catch (Exception ex)
            {
                FactoryLog.GetInstace().Error(string.Format("{0} - [{1}]", ex.Message, response.messages), ex);
                return RetornoSMS.Erro;
            }
           

          
        }
    }
}
