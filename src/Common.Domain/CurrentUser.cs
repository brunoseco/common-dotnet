using Common.Cna.Domain.Cache;
using Common.Domain.CustomExceptions;
using Common.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class CurrentUserData
    {
        public int UserId { get; set; }
        public bool OnlyUser { get; set; }
        public int ClienteId { get; set; }
        public string Token { get; set; }

    }

    enum TokenSplits
    {
        UserId = 5,
        ClienteId,
        AppId,
        IsAdmin,
        OnlyUser,
        CharFinal

    }
    public class CurrentUser
    {

        private ICache _cache;
        public string _token;

        public CurrentUser()
        {
        }

        public int UserId { get; set; }
        public bool OnlyUser { get; set; }
        public int ClienteId { get; set; }
        public int AppId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsReadOnly { get; set; }
        public bool Contingency { get; set; }

        public void SetCache(ICache cache)
        {
            this._cache = cache;
        }

        public void AddUserInfoToCache<T>(string token, T userInfo) where T : class
        {
            this._token = token;
            this._cache.Add(this._token, userInfo);

        }

        public void UpdateUserInfoToCache<T>(string token, T userInfo) where T : class
        {
            this._token = token;
            this._cache.Remove(this._token);
            this._cache.Add(this._token, userInfo);

        }

        public void RemoveFromCache(string token)
        {
            this._token = token;
            this._cache.Remove(this._token);
        }

        public CurrentUser GetUserFromCache(string token)
        {
            if (token.IsNullOrEmpty())
                throw new CustomNotAutorizedException("User Not autorized");
            
            var tokenConfig = ConfigurationManager.AppSettings["tokenConfig"] as string;
            if (tokenConfig == "JWT")
            {
                if (token.Length < 36)
                    throw new CustomBadRequestException(string.Format("token : {0} mal formado", token));

                this.UserId = this.GetUserId(token);
                this.OnlyUser = this.GetUserOnlyUser(token);
                this.ClienteId = this.GetUserClienteId(token);
                this.IsAdmin = this.GetIsAdmin(token);
                this.AppId = this.GetAppId(token);
                this._token = token;
                return this;
            }

            return this._cache.GetAndCast<CurrentUser>(token);            
        }

        public T GetUserInfoFromCache<T>(Func<CurrentUser, T> contingencyMethod) where T : class
        {
            if (this._token.IsNullOrEmpty())
                throw new CustomNotAutorizedException("User Not autorized");

            if (this._token.Length < 36)
                throw new CustomBadRequestException(string.Format("token : {0} mal formado", this._token));

            var userInfo = this._cache.GetAndCast<T>(this._token);

            var contingencyCache = ConfigurationManager.AppSettings["ContingencyCache"] as string;
            if (contingencyCache.IsNotNull() && contingencyCache.ToLower() == "true")
            {
                if (userInfo.IsNull())
                {
                    var result = contingencyMethod(this);
                    this.AddUserInfoToCache<T>(this._token, result);
                    return result;
                }
            }

            return userInfo;
        }

        private int GetUserId(string token)
        {
            var value = Convert.ToInt32(token.Split('-')[(int)TokenSplits.UserId]);
            return value;
        }


        private int GetUserClienteId(string token)
        {
            var value = Convert.ToInt32(token.Split('-')[(int)TokenSplits.ClienteId]);
            return value;
        }

        private bool GetUserOnlyUser(string token)
        {
            var value = Convert.ToBoolean(token.Split('-')[(int)TokenSplits.OnlyUser]);
            return value;
        }

        private bool GetIsAdmin(string token)
        {
            var value = Convert.ToBoolean(token.Split('-')[(int)TokenSplits.IsAdmin]);
            return value;
        }

        private int GetAppId(string token)
        {
            var value = Convert.ToInt32(token.Split('-')[(int)TokenSplits.AppId]);
            return value;
        }






    }
}
