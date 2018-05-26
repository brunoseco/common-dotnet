using Newtonsoft.Json;
using System;
using System.Web;

namespace Common.API
{
    public static class HelperCookie
    {
        public static void SetCookie(string cookieName, string value, int expiresSeconds = 50)
        {
            if (HttpContext.Current.IsNull())
                return;

            HttpContext.Current.Response.Cookies.Remove(cookieName);
            var _cookie = new HttpCookie(cookieName);
            _cookie.Value = value;
            _cookie.Expires = DateTime.Now.AddSeconds(expiresSeconds);
            HttpContext.Current.Response.Cookies.Add(_cookie);

        }

        public static void SetCookieJson(string cookieName, object value, int expiresSeconds = 50)
        {
            var _value = JsonConvert.SerializeObject(value);
            SetCookie(cookieName, _value, expiresSeconds);
        }

        public static HttpCookie GetCookie(string cookieName)
        {
            if (HttpContext.Current.IsNull())
                return null;

            var value = HttpContext.Current.Response.Cookies.Get(cookieName);
            return value;
        }
    }
}
