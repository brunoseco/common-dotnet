using Common.StorageBase;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Common.StorageManagement
{
    interface IStorageWeb : IStorageBase
    {
        int Save(IEnumerable<HttpPostedFileBase> files, string folder);
        string SaveAndRename(HttpPostedFileBase file, string fileNameWithoutExtension, string folder);

    }
}