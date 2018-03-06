using Common.Domain.Interfaces;
using Common.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sms
{
    public class SmsReluzCap : ISMS
    {

        private wsreluzcapcorp.ReluzCapWebService ServiceSmsCorporative;
        private wsreluzcapmkt.ReluzCapWebService ServiceSmsMarketing;

        private DataSet dataSet;

        public SmsReluzCap()
        {
            this.ServiceSmsCorporative = new wsreluzcapcorp.ReluzCapWebService();
            this.ServiceSmsMarketing = new wsreluzcapmkt.ReluzCapWebService();

            this.dataSet = new DataSet();

            MakeDataSet();

        }
        private void MakeDataSet()
        {
            this.dataSet = new DataSet();

            this.dataSet.Tables.Add(new DataTable("EnviaSMSDataSet"));
            this.dataSet.Tables[0].Columns.Add("seunum");
            this.dataSet.Tables[0].Columns.Add("idlote");
            this.dataSet.Tables[0].Columns.Add("celular");
            this.dataSet.Tables[0].Columns.Add("mensagem");
            this.dataSet.Tables[0].Columns.Add("agendamento");
        }

        public void Reset()
        {
            MakeDataSet();
        }

        public string User { get; set; }
        public string Password { get; set; }
        public TipoSMS Tipo { get; set; }
        public string PhoneNumberFrom { get; set; }

        public void Add(string phoneNumber, string message)
        {
            if (message.Length < 4096)
                this.dataSet.Tables[0].Rows.Add(PhoneNumberFrom, null, phoneNumber, message, null);
        }

        public RetornoSMS Send()
        {
            if (this.dataSet.Tables[0].Rows.Count < 0)
                return RetornoSMS.Erro;

            var retorno = string.Empty;

            if (Tipo == TipoSMS.Marketing)
                retorno = this.ServiceSmsMarketing.EnviaSMSDataSet(User, Password, this.dataSet);
            else
                retorno = this.ServiceSmsCorporative.EnviaSMSDataSet(User, Password, this.dataSet);

            if (retorno.StartsWith("OK"))
                return RetornoSMS.OK;
            else
                return RetornoSMS.Erro;
        }


    }
}
