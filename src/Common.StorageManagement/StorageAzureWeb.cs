using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Common.StorageBase;

namespace Common.StorageManagement
{
    class StorageAzureWeb : StorageAzureBase, IStorageWeb
    {
        public string SaveAndRename(HttpPostedFileBase file, string fileNameWithoutExtension, string folder)
        {
            if (file.ContentLength > 0)
                return SaveAndRename(file.FileName, file.InputStream, fileNameWithoutExtension, folder);

            return string.Empty;
        }

        public int Save(IEnumerable<HttpPostedFileBase> files, string folder)
        {
            var filesSaved = 0;
            foreach (var file in files)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                SaveAndRename(file, fileNameWithoutExtension, folder);
                filesSaved++;
            }
            return filesSaved;
        }

    }
}