using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.StorageBase
{
    public class HelperStorageSettings
    {


        public static string GetContentTypeTextPlain()
        {
            return System.Net.Mime.MediaTypeNames.Text.Plain;
        }

        public static string GetContentTypeXls()
        {
            return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }

        public static string GetContentTypeDownload()
        {
            return System.Net.Mime.MediaTypeNames.Application.Octet;
        }

        public static string GetContentTypePdf()
        {
            return System.Net.Mime.MediaTypeNames.Application.Pdf;
        }

    }
}