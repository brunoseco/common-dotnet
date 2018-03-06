using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Common.StorageBase
{
    public interface IStorageBase
    {
        bool Delete(string fileName, string folder, bool WithoutExtension = true);
        bool DirectoryExists(string folder);
        DownloadInfo Download(string fileName, string folder, bool WithoutExtension = true);
        bool FileExists(string fileName, string folder, bool WithoutExtension = true);
        string GetStoragePathBase();
        Stream GetStream(string fileName, string folder, bool WithoutExtension = true);
        bool Rename(string fileName, string newFileName, string folder);
        string SaveAndRename(string fileName, Stream InputStream, string fileNameWithoutExtension, string folder);
        bool SaveBytesInFile(byte[] bytes, string fileName, string folder);
    }
}