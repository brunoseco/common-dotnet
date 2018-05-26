using Common.API;
using Common.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Test;
using Common.Authentication;
using Common.Authentication.Dto;
using Common.Domain;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using Common.Infrastructure.Log;

namespace Common.Test
{
    public abstract class TestBase
    {
        protected string token;

        /// <summary>
        /// C:\\Users\\User\\AppData\\Local\\Temp\\token.seed
        /// </summary>
        /// <returns></returns>
        protected string tokenCache()
        {
            var path = Path.Combine(FolderResults(), "token.result");
            var seed = new FileInfo(path);
            if (seed.Exists)
                return File.ReadAllText(path);

            return string.Empty;

        }

        protected void tokenCache(string token)
        {
            var path = Path.Combine(FolderResults(), "token.result");
            if (!Directory.Exists(FolderResults()))
                Directory.CreateDirectory(FolderResults());

            File.WriteAllText(path, token);

        }

        public TestBase()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Auth();
        }

        protected HttpResult<THttpResult> ExecuteStep<TSeed, TResultPrevious, THttpResult>(Func<TSeed, TResultPrevious, HttpResult<THttpResult>> postData, string seedFileName, TResultPrevious assertResultPrevious)
        {
            var seed = LoadSeedData(seedFileName);
            var model = Parse<TSeed>(seed);
            var result = postData(model, assertResultPrevious);
            if (result.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(result.Errors.FirstOrDefault());

            return result;
        }

        protected HttpResult<THttpResult> ExecuteStep<TSeed, THttpResult>(Func<TSeed, HttpResult<THttpResult>> postData, string seedFileName)
        {
            var seed = LoadSeedData(seedFileName);
            var model = Parse<TSeed>(seed);
            var result = postData(model);
            
            if (result.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(result.Errors.FirstOrDefault());

            return result;
        }


        public T Parse<T>(string serialize)
        {
            var result = string.IsNullOrEmpty(serialize) ? default(T) : JsonConvert.DeserializeObject<T>(serialize);
            if (result.IsNull())
                throw new InvalidOperationException(string.Format("Dados não carregados"));
            return result;

        }


        public virtual void Auth()
        {
            var token = Guid.NewGuid().ToString();
            var cache = new FactoryCache();

            var currentUser = new CurrentUser
            {
                UserId = 32,
            };

            cache.Add(token, currentUser);
            this.token = token;
        }

        public string LoadSeedData(string fileCaseName)
        {
            var path = Path.Combine(FolderSeedData(), string.Format("{0}.{1}", fileCaseName, "seed"));
            if (File.Exists(path))
                return File.ReadAllText(path, Encoding.UTF8);

            return string.Empty;
        }
        public string LoadResult(string fileCaseName)
        {
            var path = Path.Combine(FolderResults(), string.Format("{0}.{1}", fileCaseName, "result"));
            if (File.Exists(path))
                return File.ReadAllText(path);

            return string.Empty;
        }


        public bool ExistResult(string name, bool disabled = false)
        {
            if (disabled)
                return false;

            var path = Path.Combine(FolderResults(), string.Format("{0}.{1}", name, "result"));
            return File.Exists(path);
        }

        public void SaveResult<T>(T result, string name)
        {
            var path = Path.Combine(FolderResults(), string.Format("{0}.{1}", name, "result"));
            var serialize = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            using (var stream = new StreamWriter(path))
            {
                stream.Write(serialize);
            }

        }



        protected static string MakeStringValueSuccess()
        {
            return string.Format("String Value Success {0}", DateTime.Now.Millisecond);

        }
        protected static string MakeStringValueSuccess(int length)
        {
            var moqValue = string.Format("String Value Success {0}", DateTime.Now.Millisecond);
            var result = moqValue.Length > length ? moqValue.Substring(0, length) : moqValue;
            return result;
        }

        protected static string MakeCnpjValid()
        {
            var randomizadorInteiros = new Random();

            var n = 10;
            var n1 = randomizadorInteiros.Next(n);
            var n2 = randomizadorInteiros.Next(n);
            var n3 = randomizadorInteiros.Next(n);
            var n4 = randomizadorInteiros.Next(n);
            var n5 = randomizadorInteiros.Next(n);
            var n6 = randomizadorInteiros.Next(n);
            var n7 = randomizadorInteiros.Next(n);
            var n8 = randomizadorInteiros.Next(n);;
            var n9 = randomizadorInteiros.Next(n);
            var n10 = randomizadorInteiros.Next(n);
            var n11 = randomizadorInteiros.Next(n);
            var n12 = randomizadorInteiros.Next(n);
            var d1 = n12 * 2 + n11 * 3 + n10 * 4 + n9 * 5 + n8 * 6 + n7 * 7 + n6 * 8 + n5 * 9 + n4 * 2 + n3 * 3 + n2 * 4 + n1 * 5;
           
            d1 = 11 - (d1 % 11);
            
            if (d1 >= 10) 
                d1 = 0;
            
            var d2 = d1 * 2 + n12 * 3 + n11 * 4 + n10 * 5 + n9 * 6 + n8 * 7 + n7 * 8 + n6 * 9 + n5 * 2 + n4 * 3 + n3 * 4 + n2 * 5 + n1 * 6;
            
            d2 = 11 - (d2 % 11);
            
            if (d2 >= 10)
                d2 = 0;
            
            var cnpj = ""+n1+""+n2+""+n3+""+n4+""+n5+""+n6+""+n7+""+n8+""+n9+""+n10+""+n11+""+n12+""+d1+""+d2;

            return cnpj.ToString();
        }


        protected static string MakeCPFValid()
        {
            int soma = 0, resto = 0;
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            Random rnd = new Random();
            string semente = rnd.Next(100000000, 999999999).ToString();

            for (int i = 0; i < 9; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente = semente + resto;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente = semente + resto;
            
            System.Threading.Thread.Sleep(2000);

            return semente;

        }

        protected static string MakeEmailValid()
        {
            var generatedEmail = string.Format("{0}@{1}{2}.com.br", MakeIntValueSuccess(), MakeIntValueSuccess(), DateTime.Now.ToString());
            generatedEmail = generatedEmail.Replace("/", "");
            generatedEmail = generatedEmail.Replace(":", "");
            generatedEmail = generatedEmail.Replace(" ", String.Empty);   
            return generatedEmail;
        }
        protected static string MakeIEValid()
        {
            return "344119129";

        }
        protected static int MakeIntValueSuccess()
        {
            return DateTime.Now.Millisecond;

        }


        protected static DateTime MakeDateTimeValueSuccess()
        {
            return DateTime.Now;

        }
        protected static Decimal MakeDecimalValueSuccess()
        {
            return DateTime.Now.Millisecond / 0.3M;

        }

        protected static float MakeFloatValueSuccess()
        {
            return float.MaxValue;

        }

        protected static bool MakeBoolValueSuccess()
        {
            return true;

        }

        protected bool AssertDeleteSuccessOrConflited(System.Net.HttpStatusCode StatusCode, IEnumerable<string> Errors)
        {
            return StatusCode == System.Net.HttpStatusCode.OK || (StatusCode == System.Net.HttpStatusCode.InternalServerError && Errors.Where(_ => ContainsConflictedDelete(_)).Any());
        }

        protected bool ContainsConflictedDelete(string msg)
        {
            return msg.Contains("The DELETE statement conflicted with the REFERENCE constraint");
        }

        protected static string FolderSeedData()
        {
            return Path.Combine(ConfigurationManager.AppSettings["pathSeed"]);
        }


        protected static string FolderResults()
        {
            return Path.GetTempPath();
        }

        public void DeleteUsedFile(string fileName)
        {
            File.Delete(Path.Combine(FolderSeedData(), string.Format("{0}.{1}", fileName, "seed")));
        }
    }
}
