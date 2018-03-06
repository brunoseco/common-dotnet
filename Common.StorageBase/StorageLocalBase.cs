using Common.StorageBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.StorageBase
{
    public class StorageLocalBase : IStorageBase
    {

        protected string _storagePathBase;

        protected string _storagePathBaseRelative;

        public StorageLocalBase()
        {
            _storagePathBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\storage\");
            _storagePathBaseRelative = "~/Content/storage/";
        }

        public string GetStoragePathBaseRelative()
        {
            return _storagePathBaseRelative;
        }


        public string GetStoragePathBase()
        {
            return _storagePathBase;
        }

        public string SaveAndRename(string fileName, Stream InputStream, string fileNameWithoutExtension, string folder)
        {
            var extension = Path.GetExtension(fileName);
            var uploadPath = FullPathFolder(folder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var newFileName = string.Format("{0}{1}", fileNameWithoutExtension, extension);
            string filePath = Path.Combine(uploadPath, newFileName);

            Delete(fileNameWithoutExtension, folder);
            SaveStream(InputStream, filePath);

            return newFileName;
        }

        public bool Rename(string fileName, string newFileName, string folder)
        {

            var uploadPath = FullPathFolder(folder);
            var newFileNameWithExtension = string.Format("{0}{1}", newFileName, Path.GetExtension(fileName));
            var pathDestionation = Path.Combine(uploadPath, newFileNameWithExtension);
            var pathSource = Path.Combine(uploadPath, fileName);
            System.IO.File.Copy(pathSource, pathDestionation, true);
            System.IO.File.Delete(pathSource);

            return true;

        }

        public bool Delete(string fileName, string folder, bool WithoutExtension = true)
        {
            if (!WithoutExtension)
            {
                var file = GetFile(fileName, folder);

                if (file != null)
                    file.Delete();

            }
            var files = GetFilesByFileNameWithoutExtension(fileName, folder);

            if (files.Any())
            {
                foreach (var item in files)
                    item.Delete();
            }

            return true;

        }

        public Stream GetStream(string fileName, string folder, bool WithoutExtension = true)
        {
            if (!WithoutExtension)
            {

                var file = GetFile(fileName, folder);
                if (file != null)
                    return new StreamReader(file.FullName).BaseStream;

            }

            if (WithoutExtension)
            {
                var files = GetFilesByFileNameWithoutExtension(fileName, folder);

                if (files.Any())
                {
                    var file = files.FirstOrDefault();
                    return new StreamReader(file.FullName).BaseStream;
                }
            }

            return null;
        }

        public DownloadInfo Download(string fileName, string folder, bool WithoutExtension = true)
        {

            if (!WithoutExtension)
            {
                var file = GetFile(fileName, folder);

                if (file != null)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(file.FullName);

                    return new DownloadInfo
                    {

                        Bytes = fileBytes,
                        FileName = file.Name,
                    };
                }

            }

            if (WithoutExtension)
            {
                var files = GetFilesByFileNameWithoutExtension(fileName, folder);

                if (files.Any())
                {
                    var file = files.FirstOrDefault();
                    byte[] fileBytes = System.IO.File.ReadAllBytes(file.FullName);

                    return new DownloadInfo
                    {

                        Bytes = fileBytes,
                        FileName = file.Name,
                    };
                }
            }


            return new DownloadInfo
            {
                Bytes = new byte[] { },
                FileName = string.Empty,
            }; ;

        }

        public bool FileExists(string fileName, string folder, bool WithoutExtension = true)
        {
            var uploadPath = FullPathFolder(folder);
            if (Directory.Exists(uploadPath))
            {
                if (!WithoutExtension)
                {
                    var file = GetFile(fileName, folder);
                    if (file != null)
                        return true;
                }

                if (WithoutExtension)
                {
                    var files = GetFilesByFileNameWithoutExtension(fileName, folder);
                    if (files.Any())
                        return true;
                }
            }

            return false;
        }

        public bool SaveBytesInFile(byte[] bytes, string fileName, string folder)
        {
            var uploadFolder = FullPathFolder(folder); ;
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var pathFile = Path.Combine(uploadFolder, fileName);

            System.IO.File.WriteAllBytes(pathFile, bytes);

            return true;
        }

        public bool DirectoryExists(string folder)
        {
            var folderPath = FullPathFolder(folder);
            return Directory.Exists(folderPath);
        }



        #region helpers

        private string FullPathFolder(string folder)
        {
            return Path.Combine(GetStoragePathBase(), folder);
        }

        private void SaveStream(Stream stream, string pathh)
        {
            using (var fileStream = new FileStream(pathh, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }

        private IEnumerable<FileInfo> GetFilesByFileNameWithoutExtension(string fileNameWithoutExtension, string folder)
        {
            var uploadPath = FullPathFolder(folder);
            return new DirectoryInfo(uploadPath)
                           .GetFiles()
                           .Where(_ => (_.Extension.Length > 0 ? _.Name.Replace(_.Extension, string.Empty) : _.Name) == fileNameWithoutExtension);
        }

        private FileInfo GetFile(string fileName, string folder)
        {
            var uploadPath = FullPathFolder(folder);
            return new DirectoryInfo(uploadPath)
                           .GetFiles()
                           .Where(_ => _.Name == fileName).SingleOrDefault();
        }

        #endregion
    }
}