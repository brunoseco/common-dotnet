using Common.StorageBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace Common.StorageManagement
{
   
    public static class HelperStorageWeb
    {
        private static IStorageWeb _storage;
        static HelperStorageWeb()
        {
            _storage = new StorageLocalWeb();
            if (ConfigurationManager.AppSettings["Storage"].ToLower() == "cloud")
                _storage = new StorageAzureWeb();
        }
      

        public static string SaveAndRename(HttpPostedFileBase file, string fileNameWithoutExtension, string folder)
        {
            return _storage.SaveAndRename(file, fileNameWithoutExtension, folder);
        }

        public static int Save(IEnumerable<HttpPostedFileBase> files, string folder)
        {
            return _storage.Save(files, folder);
        }

        public static int Save(HttpPostedFileBase file, string folder)
        {
            return Save(new List<HttpPostedFileBase> { file }, folder);
        }

        public static int Save(HttpFileCollectionBase files, string folder)
        {
            return Save(files, folder);
        }

       }
}