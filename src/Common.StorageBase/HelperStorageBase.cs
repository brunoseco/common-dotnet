using Common.StorageBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace Common.StorageBase
{
   
    public static class HelperStorageBase
    {
        private static IStorageBase _storage;
        static HelperStorageBase()
        {
            _storage = new StorageLocalBase();
            if (ConfigurationManager.AppSettings["Storage"].ToLower() == "cloud")
                _storage = new StorageAzureBase();
        }
        public static string GetStoragePathBase()
        {
            return _storage.GetStoragePathBase();
        }

        public static string GetContentTypeTextPlain()
        {
            return HelperStorageSettings.GetContentTypeTextPlain();
        }

        public static string GetContentTypeXls()
        {
            return HelperStorageSettings.GetContentTypeXls();
        }

        public static string GetContentTypeDownload()
        {
            return HelperStorageSettings.GetContentTypeDownload();
        }

        public static string GetContentTypePdf()
        {
            return HelperStorageSettings.GetContentTypePdf();
        }

        public static string SaveAndRename(string fileName, Stream inputStream, string fileNameWithoutExtension, string folder)
        {
            return _storage.SaveAndRename(fileName, inputStream, fileNameWithoutExtension, folder);
        }
      
        public static bool Rename(string fileName, string newFileName, string folder)
        {
            return _storage.Rename(fileName, newFileName, folder);
        }

        public static bool Delete(string fileName, string folder, bool WithoutExtension = true)
        {
            return _storage.Delete(fileName, folder, WithoutExtension);
        }

        public static DownloadInfo Download(string fileName, string folder, bool WithoutExtension = true)
        {
            return _storage.Download(fileName, folder, WithoutExtension);
        }

        public static Stream GetStream(string fileName, string folder, bool WithoutExtension = true)
        {
            return _storage.GetStream(fileName, folder, WithoutExtension);
        }

        public static bool FileExists(string fileName, string folder, bool WithoutExtension = true)
        {
            return _storage.FileExists(fileName, folder, WithoutExtension);
        }

        public static bool SaveBytesInFile(byte[] bytes, string fileName, string folder)
        {
            return _storage.SaveBytesInFile(bytes, fileName, folder);
        }

        public static bool DirectoryExists(string folder)
        {
            return _storage.DirectoryExists(folder);
        }
    }
}