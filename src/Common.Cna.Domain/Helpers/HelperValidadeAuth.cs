using Common.Domain;
using Common.Domain.Helper;
using Common.Domain.Interfaces;
using System;

namespace Common.Cna.Domain.Helpers
{
    public static class HelperValidateAuth
    {
        private static string _simple { get { return "{0}-S"; } }
        private static string _menu { get { return "{0}-MN"; } }
        private static string _permissions { get { return "{0}-PR"; } }
        private static string _parameters { get { return "{0}-P"; } }


        public static CurrentUser ValidateAuthSimple(string token, ICache cache)
        {
            return ValidateAuth(TokenSimple(token), cache);
        }

        public static CurrentUser ValidateAuthPermissions(string token, ICache cache)
        {
            return ValidateAuth(TokenPermissions(token), cache);
        }

        public static CurrentUser ValidateAuthMenu(string token, ICache cache)
        {
            return ValidateAuth(TokenMenu(token), cache);
        }

        public static CurrentUser ValidateAuthParameters(string token, ICache cache)
        {
            return ValidateAuth(TokenParameters(token), cache);
        }

        private static CurrentUser ValidateAuth(string token, ICache cache)
        {
            return HelperCurrentUser.ValidateAuth(token, cache);
        }

        public static string TokenSimple(string token)
        {
            return string.Format(_simple, token);
        }

        public static string TokenPermissions(string token)
        {
            return string.Format(_permissions, token);
        }

        public static string TokenMenu(string token)
        {
            return string.Format(_menu, token);
        }


        public static string TokenParameters(string token)
        {
            return string.Format(_parameters, token);
        }

        public static string MakeTokenInfo(string guid, int userId, int clienteId, int appId, bool isAdmin, bool onlyUser = false)
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}", guid, userId, clienteId, appId, isAdmin, onlyUser);
        }

    }
}
