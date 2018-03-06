using Common.StorageBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.StorageManagement
{
    class StorageLocalWeb : StorageLocalBase, IStorageWeb
    {
        public string SaveAndRename(HttpPostedFileBase file, string fileNameWithoutExtension, string folder)
        {
            var fileSaved = string.Empty;
            if (file.ContentLength > 0)
                return SaveAndRename(file.FileName, file.InputStream, fileNameWithoutExtension, folder);

            return fileSaved;
        }

        public int Save(IEnumerable<HttpPostedFileBase> files, string folder)
        {
            var filesSaved = 0;
            foreach (HttpPostedFileBase file in files)
            {
                if (file.ContentLength > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var uploadPath = HttpContext.Current.Server.MapPath(Path.Combine(GetStoragePathBaseRelative(), folder));

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string filePath = Path.Combine(uploadPath, file.FileName);
                    file.SaveAs(filePath);
                    filesSaved++;
                }
            }
            return filesSaved;
        }

    }
}