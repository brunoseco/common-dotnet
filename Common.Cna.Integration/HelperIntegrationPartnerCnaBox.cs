using Common.API;
using Common.Domain.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

namespace Common.Cna.Integration
{
    public static class HelperIntegrationPartnerCnaBox
    {

        private static string _tokenIntegrationPartner;

        private static string _email;
        private static string _password;

        public static string EndPointIntegrationPartnerCnaBox()
        {
            return ConfigurationManager.AppSettings["endPointIntegrationPartnerCnaBox"];
        }

        public static void Auth(string email, string password)
        {
            _email = email;
            _password = password;
            _tokenIntegrationPartner = string.Empty;
        }

        public static string GetToken()
        {
            if (!_tokenIntegrationPartner.IsNullOrEmpaty())
                return _tokenIntegrationPartner;

            var endPointIntegrationPartnerCnaBox = EndPointIntegrationPartnerCnaBox();
            var client = new HelperHttp(endPointIntegrationPartnerCnaBox);

            var urlAuth = string.Format("{0}/Api/UsuarioParceiro", endPointIntegrationPartnerCnaBox);

            try
            {
                var response = client.PostBasic<dynamic, dynamic>(urlAuth, new
                {
                    Email = _email ?? "teste",
                    Password = _password ?? "teste",
                });

                if (Convert.ToInt32(response.StatusCode) == 200)
                {
                    _tokenIntegrationPartner = response.Data.Token;
                    return _tokenIntegrationPartner;
                }
            }
            catch (Exception ex)
            {
                throw new CustomValidationException(string.Format("Erro ao na autenticação de integração usuario:{0} - 'Não Autenticado' [{1}] - [{2}]", _email, ex.Message, ex.StackTrace));
            }


            throw new CustomValidationException(string.Format("Erro ao na autenticação de integração usuario:{0} - 'Não Autenticado'", _email));

        }

        public static string GetAlunoResponsavel(string token)
        {
            var endPointIntegrationPartnerCnaBox = EndPointIntegrationPartnerCnaBox();
            var client = new HelperHttp(endPointIntegrationPartnerCnaBox);

            var urlAuth = string.Format("{0}/Api/AlunosResponsavel", endPointIntegrationPartnerCnaBox);

            try
            {
                client.AddCustomHeaders("token:" + token);

                var response = client.GetBasic<dynamic>(urlAuth, new QueryStringParameter().Add("CPF_CNPJ", "43139842830"));
            }
            catch (Exception ex)
            {
                throw new CustomValidationException(string.Format("Erro ao na autenticação de integração usuario:{0} - 'Não Autenticado' [{1}] - [{2}]", _email, ex.Message, ex.StackTrace));
            }


            throw new CustomValidationException(string.Format("Erro ao na autenticação de integração usuario:{0} - 'Não Autenticado'", _email));

        }

        public static string GetResponsavel(string token)
        {
            var endPointIntegrationPartnerCnaBox = EndPointIntegrationPartnerCnaBox();
            var client = new HelperHttp(endPointIntegrationPartnerCnaBox);

            var urlAuth = string.Format("{0}/Api/Responsavel", endPointIntegrationPartnerCnaBox);

            try
            {
                client.AddCustomHeaders("token:" + token);

                var response = client.GetBasic<dynamic>(urlAuth, new QueryStringParameter().Add("CPF_CNPJ", "43139842830"));
            }
            catch (Exception ex)
            {
                throw new CustomValidationException(string.Format("Erro ao na autenticação de integração usuario:{0} - 'Não Autenticado' [{1}] - [{2}]", _email, ex.Message, ex.StackTrace));
            }


            throw new CustomValidationException(string.Format("Erro ao na autenticação de integração usuario:{0} - 'Não Autenticado'", _email));
        }

    }
}
