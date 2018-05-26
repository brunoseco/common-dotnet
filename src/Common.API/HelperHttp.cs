using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.API
{
    public class HelperHttp
    {

        public class HttpHeaderParameters {

            public string Key { get; set; }
            public string Value { get; set; }
            public string ValueModifier { get; set; }

        }


        private string baseAddress;
        private List<HttpHeaderParameters> customHeaders;

        public HelperHttp(string baseAddress)
        {
            this.baseAddress = baseAddress;
            this.customHeaders = new List<HttpHeaderParameters>();
        }

        public void AddCustomHeaders(string key, string value)
        {
            customHeaders.Add(new HttpHeaderParameters {
                Key  = key,
                Value = value
            });
        }

        public void AddBearerAuthorization(string value)
        {
            customHeaders.Add(new HttpHeaderParameters
            {
                Key = "Authorization",
                Value = value,
                ValueModifier = "Bearer"
            });
        }

        public void AddBasicAuthorization(string value)
        {
            customHeaders.Add(new HttpHeaderParameters
            {
                Key = "Authorization",
                Value = value,
                ValueModifier = "Basic"
            });
        }

        public void AddCustomHeaders(string header)
        {
            var headerKey = header.Split(':')[0].Trim();
            var headerValue = header.Split(':')[1].Trim();

            customHeaders.Add(new HttpHeaderParameters {
                Key = headerKey,
                Value = headerValue,
            });
        }

        public HttpResult<TReturn> Get<TReturn, TFilter>(string token, string resource, TFilter model, bool EnabledExceptions = true)
        {
            return Get<TReturn>(token, resource, model.ToQueryString(), EnabledExceptions);
        }
        public HttpResult<T> Get<T>(string resource, QueryStringParameter queryParameters = null)
        {
            return Get<T>(string.Empty, resource);
        }
        public HttpResult<T> Get<T>(string token, string resource, QueryStringParameter queryParameters = null, bool EnabledExceptions = true)
        {
            if (EnabledExceptions)
                return GetHttpClient<T>(token, resource, queryParameters);

            return GetWebClient<T>(token, resource, queryParameters);
        }
        public HttpResult<TResult> Post<T, TResult>(string resource, T model)
        {
            return Post<T, TResult>(string.Empty, resource, model);

        }
        public HttpResult<TResult> Post<T, TResult>(string token, string resource, T model)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("token", token);

                    var response = client.PostAsJsonAsync(resource, model).Result;
                    var result = response.Content.ReadAsAsync<HttpResult<TResult>>().Result;
                    return result;

                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<TResult>(resource, ex);
                }
            }

        }
        public HttpResult<TResult> Put<T, TResult>(string resource, T model)
        {
            return Put<T, TResult>(string.Empty, resource, model);
        }
        public HttpResult<TResult> Put<T, TResult>(string token, string resource, T model)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("token", token);

                    var response = client.PutAsJsonAsync(resource, model).Result;
                    var result = response.Content.ReadAsAsync<HttpResult<TResult>>().Result;
                    return result;

                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<TResult>(resource, ex);
                }
            }

        }
        public HttpResult<TResult> Delete<T, TResult>(string token, string resource, T model)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("token", token);

                    resource = MakeResource(resource, model.ToQueryString());


                    var response = client.DeleteAsync(resource).Result;
                    var result = response.Content.ReadAsAsync<HttpResult<TResult>>().Result;
                    return result;

                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<TResult>(resource, ex);
                }
            }



        }

        public TResult PostBasic<T, TResult>(string resource, T model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.baseAddress);
                client.DefaultRequestHeaders.Clear();

                //if (headers.IsAny())
                //    AddHeaderInClientRequest(headers, client);

                if (this.customHeaders.IsAny())
                    AddHeaderInClientRequest(this.customHeaders, client);

                var response = client.PostAsJsonAsync(resource, model).Result;
                var result = response.Content.ReadAsAsync<TResult>().Result;
                return result;

            }

        }
        public TResult GetBasic<TResult>(string resource, QueryStringParameter queryParameters = null)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(this.baseAddress);

                if (this.customHeaders.IsAny())
                    AddHeaderInClientRequest(this.customHeaders, client);

                resource = MakeResource(resource, queryParameters);


                var response = client.GetAsync(resource).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsAsync<TResult>().Result;
                    return result;
                }

                return default(TResult);

            }
        }
        private HttpResult<TResult> MakeErrorHttpResult<TResult>(string resource, Exception ex)
        {
            return new HttpResult<TResult>().Error(string.Format("Erro ao acessar a API {0}{1}-Error: {2}", this.baseAddress, resource, ex));
        }
        private string MakeResource(string resource, QueryStringParameter queryParameters)
        {
            if (queryParameters != null)
            {
                var queryStringUrl = queryParameters.Get().ToQueryString();
                resource = String.Concat(resource, queryStringUrl);
            }
            return resource;
        }
        private HttpResult<T> GetWebClient<T>(string token, string resource, QueryStringParameter queryParameters)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = this.baseAddress;
                client.Headers.Add("token", token);
                client.Encoding = Encoding.UTF8;

                try
                {
                    resource = MakeResource(resource, queryParameters);

                    var result = client.DownloadString(resource);
                    return string.IsNullOrEmpty(result) ? default(HttpResult<T>) : JsonConvert.DeserializeObject<HttpResult<T>>(result);
                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<T>(resource, ex);
                }
            }
        }
        private HttpResult<T> GetHttpClient<T>(string token, string resource, QueryStringParameter queryParameters)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("token", token);

                    if (this.customHeaders.IsAny())
                        AddHeaderInClientRequest(this.customHeaders, client);

                    resource = MakeResource(resource, queryParameters);

                    var response = client.GetAsync(resource).Result;

                    var result = response.Content.ReadAsStringAsync();
                    var model = string.IsNullOrEmpty(result.Result) ? default(HttpResult<T>) : JsonConvert.DeserializeObject<HttpResult<T>>(result.Result);
                    return model;

                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<T>(resource, ex);
                }
            }
        }
        private static void AddHeaderInClientRequest(List<HttpHeaderParameters> headers, HttpClient client)
        {
            if (headers.IsAny())
            {
                foreach (var header in headers)
                {
                    var headerKey = header.Key;
                    var headerValue = header.Value;
                    var valueModifier = header.ValueModifier;

                    if (headerKey == "Content-Type")
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(headerValue));
                    else if (headerKey == "Authorization")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(valueModifier, headerValue);
                    else
                        client.DefaultRequestHeaders.Add(headerKey, headerValue);
                }
            }
        }

    }
}
