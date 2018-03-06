using Common.API;
using Common.Domain.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

namespace Common.Integration
{
    public static class HelperIntegrationPartnerCnaBox
    {

        private static string _tokenIntegrationPartner;

        private static string _email;
        private static string _password;

        private static string EndPointIntegrationPartnerCnaBox()
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

            var endPointIntegrationPartnerCnaBox = HelperIntegrationPartnerCnaBox.EndPointIntegrationPartnerCnaBox();
            var client = new HelperHttp(endPointIntegrationPartnerCnaBox);

            var urlAuth = string.Format("{0}/Api/UsuarioParceiro", endPointIntegrationPartnerCnaBox);

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


            throw new CustomValidationException("Erro ao na autenticação de integração - 'Não Autenticado'");

        }

    }
}
