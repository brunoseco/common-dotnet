using Common.Domain.CustomExceptions;
using Common.Domain.Interfaces;
using System;
using System.Configuration;

namespace Common.Domain.Helper
{
    public static class HelperCurrentUser
    {

        public static CurrentUser ValidateAuth(string token, ICache cache)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DisableAuth"]) == true)
                return new CurrentUser();

            var user = new CurrentUser();
            user.SetCache(cache);
            user.GetUserFromCache(token);

            if (user.IsNull())
                throw new CustomNotAutorizedException(string.Format("Usuário [{0}] não autenticado", token));

            return user;
        }        
    }
}
