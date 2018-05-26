using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.API
{
    public static class BasicAuth
    {

        public static string Base64Encoding(string source)
        {
            var  encoding = new System.Text.ASCIIEncoding();
            var bytes = encoding.GetBytes(source);
            var s = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
            return s;
        }

    }
}
