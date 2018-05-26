using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StorageBase
{
    public class StorageAzureBase : IStorageBase
    {
        private string _share;
        private string _storagePathBase;


        public StorageAzureBase()
        {
            _storagePathBase = GetStorageAccount().FileEndpoint.AbsoluteUri;
            _share = (CloudConfigurationManager.GetSetting("Share") ?? "geral").ToLower();
            VerifyConfiguration();
        }

        public string GetStoragePathBase()
        {
            return _storagePathBase;
        }

        public string SaveAndRename(string fileName, Stream InputStream, string fileNameWithoutExtension, string folder)
        {
            var extension = Path.GetExtension(fileName);
            var NewFileName = string.Format("{0}{1}", fileNameWithoutExtension, extension);
            UploadStream(InputStream, NewFileName, folder);
            return NewFileName;


        }

        public bool Rename(string fileName, string newFileName, string folder)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string fileName, string folder, bool WithoutExtension = true)
        {
            if (!WithoutExtension)
            {
                var file = GetFile(fileName, folder);
                if (file != null)
                    file.Delete();
            }

            if (WithoutExtension)
            {
                var files = GetFilesByFileNameWithoutExtension(fileName, folder);
                if (files.Any())
                {
                    foreach (var fileItem in files)
                        fileItem.Delete();
                }
            }

            return true;

        }

        public DownloadInfo Download(string fileName, string folder, bool WithoutExtension = true)
        {
            if (!WithoutExtension)
            {
                var file = GetFile(fileName, folder);
                if (file != null)
                {
                    var fileBytes = GetBytes(file);
                    return new DownloadInfo
                    {
                        Bytes = fileBytes,
                        FileName = GetFileNameByFileItem(file),
                    };
                }
            }

            if (WithoutExtension)
            {
                var files = GetFilesByFileNameWithoutExtension(fileName, folder);
                if (files.Any())
                {
                    var fileItem = files.FirstOrDefault();
                    var fileBytes = GetBytes(fileItem);
                    return new DownloadInfo
                    {
                        Bytes = fileBytes,
                        FileName = GetFileNameByFileItem(fileItem),
                    };
                }
            }

            return new DownloadInfo
            {
                Bytes = new byte[] { },
                FileName = string.Empty,
            };
        }

        public Stream GetStream(string fileName, string folder, bool WithoutExtension = true)
        {
            if (!WithoutExtension)
            {
                var file = GetFile(fileName, folder);
                if (file != null)
                    return GetStream(file);
            }

            if (WithoutExtension)
            {
                var files = GetFilesByFileNameWithoutExtension(fileName, folder);
                if (files.Any())
                {
                    var fileItem = files.FirstOrDefault();
                    return GetStream(fileItem);
                }
            }

            return null;
        }

        public bool FileExists(string fileName, string folder, bool WithoutExtension = true)
        {
            if (WithoutExtension)
                return GetFilesByFileNameWithoutExtension(fileName, folder).Any();

            if (WithoutExtension)
                return GetFile(fileName, folder) != null;

            return false;
        }

        public bool SaveBytesInFile(byte[] bytes, string fileName, string folder)
        {
            UploadBytes(bytes, fileName, folder);
            return true;
        }

        public bool DirectoryExists(string folder)
        {
            var root = GetShare().GetRootDirectoryReference();
            var dir = root.GetDirectoryReference(folder.ToLower());
            return dir.Exists();
        }



        #region helpers


        private MemoryStream GetStream(CloudFile cloadFile)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                cloadFile.DownloadToStream(ms);
                return ms;
            }
        }

        private byte[] GetBytes(CloudFile cloadFile)
        {
            return GetStream(cloadFile).ToArray();
        }

        private bool VerifyConfiguration()
        {
            bool configOK = true;
            string connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            if (String.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("[AccountName]") || connectionString.Contains("[AccountKey]"))
            {
                configOK = false;
                throw new InvalidOperationException("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
            }
            return configOK;
        }


        private CloudFile GetCloadFileByFileItem(string folder, IListFileItem fileItem)
        {
            string fileName = GetFileNameByFileItem(fileItem);
            var cloadFile = GetDirectory(folder).GetFileReference(fileName);
            return cloadFile;
        }

        private string GetFileNameByFileItem(IListFileItem fileItem)
        {
            return fileItem.Uri.AbsolutePath;
        }

        private IEnumerable<CloudFile> GetFilesByFileNameWithoutExtension(string fileNameWithoutExtension, string folder)
        {
            return GetFiles(folder)
                .Where(_ => _.Name.Replace(Path.GetExtension(_.Name), string.Empty) == fileNameWithoutExtension);
        }

        private CloudFile GetFile(string fileName, string folder)
        {
            return GetFiles(folder).Where(_ => _.Name == fileName).SingleOrDefault();
        }

        private CloudStorageAccount GetStorageAccount()
        {
            return CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        private CloudFileClient GetFileClient()
        {
            return GetStorageAccount().CreateCloudFileClient();
        }

        private CloudFileShare GetShare()
        {
            var share = GetFileClient().GetShareReference(_share);

            try
            {
                share.CreateIfNotExists();
            }
            catch (StorageException)
            {
                throw new InvalidOperationException("Please make sure your storage account has storage file endpoint enabled and specified correctly in the app.config - then restart the sample.");
            }

            return share;
        }

        private CloudFileDirectory GetDirectory(string folder)
        {
            var root = GetShare().GetRootDirectoryReference();
            var dir = root.GetDirectoryReference(folder.ToLower());
            dir.CreateIfNotExists();
            return dir;

        }

        private void UploadStream(Stream source, string fileName, string folder)
        {
            //var file = GetFile(fileName, folder);
            var file = GetDirectory(folder).GetFileReference(fileName);
            file.UploadFromStream(source);
        }

        private void UploadBytes(byte[] source, string fileName, string folder)
        {
            //var file = GetFile(fileName, folder);
            var file = GetDirectory(folder).GetFileReference(fileName);
            file.UploadFromByteArray(source, 0, source.Length);
        }

        private IEnumerable<CloudFile> GetFiles(string folder)
        {
            var results = new List<CloudFile>();
            FileContinuationToken token = null;
            do
            {
                var resultSegment = GetDirectory(folder)
                    .ListFilesAndDirectoriesSegmented(token);

                foreach (var result in resultSegment.Results)
                {
                    var cloadFiles = result as CloudFile;
                    if (cloadFiles != null)
                        results.Add(cloadFiles);
                }
                token = resultSegment.ContinuationToken;
            }
            while (token != null);
            return results;
        }

        #endregion helpers



    }
}
