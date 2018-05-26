using Common.Cna.Domain.Cache;
using Common.Domain;
using Common.Domain.Interfaces;
using System;
using System.Configuration;

namespace Common.Cna.Domain.Helpers
{
    public static class HelperAuthFake
    {

        public static string AuthUserExecuteAsyncTask(ICache cache)
        {
            var token = Guid.NewGuid().ToString();
            var tokenS = HelperValidateAuth.TokenSimple(token);

            var currentUser = new CurrentUser { UserId = Convert.ToInt32(ConfigurationManager.AppSettings["userExecuteAsyncTask"]) };
            cache.Add(tokenS, currentUser, true);

            return token;
        }

        public static string ChkAuthUserExecuteAsyncTask(string token, ICache cache)
        {

            var tokenS = HelperValidateAuth.TokenSimple(token);
            var currentUser = new CurrentUser { UserId = Convert.ToInt32(ConfigurationManager.AppSettings["userExecuteAsyncTask"]) };

            if (!cache.ExistsKey<CurrentUser>(tokenS))
                cache.Add(tokenS, currentUser, true);

            return token;
        }

        public static bool ChkCache(string token, ICache cache)
        {
            var tokenS = HelperValidateAuth.TokenSimple(token);
            if (cache.ExistsKey<CurrentUser>(tokenS))
                return true;

            return false;
        }

        public static void AddParamtersToUserSimple(string token, ICache cache, ParametersCache parameters, CurrentUser currentUserMinimal)
        {
            var currentUserParameters = new CurrentUser()
            {
                UserId = currentUserMinimal.UserId,
            };
            var _token = HelperValidateAuth.TokenParameters(token);
            currentUserParameters.SetCache(cache);
            currentUserParameters.AddUserInfoToCache<ParametersCache>(_token, parameters);
        }

        public static void AddMoreInfoToUserSimple(string token, ICache cache, int escolaId, CurrentUser currentUserMinimal)
        {
            var _token = HelperValidateAuth.TokenSimple(token);
            currentUserMinimal.AddUserInfoToCache<ColaboradorLogadoCache>(_token, new ColaboradorLogadoCache
            {
                ColaboradorId = currentUserMinimal.UserId,
                EscolaLogada = new EscolaCache
                {
                    EscolaId = escolaId,
                }
            });
            currentUserMinimal.SetCache(cache);

        }



    }
}