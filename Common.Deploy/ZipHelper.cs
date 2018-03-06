using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;

/// <summary>
/// http://www.codeproject.com/Tips/315115/Zip-using-System-IO-Compression
/// </summary>
public static class ZipHelper
{
    public static void ZipFiles(string path, IEnumerable<string> files,
           CompressionOption compressionLevel = CompressionOption.Normal)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            ZipHelper.ZipFilesToStream(fileStream, files, compressionLevel);
        }
    }

    public static byte[] ZipFilesToByteArray(IEnumerable<string> files,
           CompressionOption compressionLevel = CompressionOption.Normal)
    {
        byte[] zipBytes = default(byte[]);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            ZipHelper.ZipFilesToStream(memoryStream, files, compressionLevel);
            memoryStream.Flush();
            zipBytes = memoryStream.ToArray();
        }

        return zipBytes;
    }

    public static void Unzip(string zipPath, string baseFolderForUnzip, string baseFolderZip = null)
    {
        using (FileStream fileStream = new FileStream(zipPath, FileMode.Open))
        {
            ZipHelper.UnzipFilesFromStream(fileStream, baseFolderForUnzip, zipPath, baseFolderZip);
        }
    }

    public static void UnzipFromByteArray(byte[] zipData, string baseFolder)
    {
        using (MemoryStream memoryStream = new MemoryStream(zipData))
        {
            ZipHelper.UnzipFilesFromStream(memoryStream, baseFolder);
        }
    }

    private static void ZipFilesToStream(Stream destination, IEnumerable<string> files, CompressionOption compressionLevel)
    {
        var filesUri = new List<Uri>();
        using (Package package = Package.Open(destination, FileMode.Create))
        {
            foreach (string path in files)
            {
                // fix for white spaces in file names (by ErrCode)
                //Uri fileUri = PackUriHelper.CreatePartUri(new Uri(@"/" +
                //              Path.GetFileName(path), UriKind.Relative));

                Uri fileUri = PackUriHelper.CreatePartUri(new Uri(path, UriKind.Relative));

                if (filesUri.Where(_ => _.OriginalString == fileUri.OriginalString).NotIsAny())
                {
                    filesUri.Add(fileUri);
                    string contentType = @"data/" + ZipHelper.GetFileExtentionName(path);
                    using (Stream zipStream = package.CreatePart(fileUri, contentType, compressionLevel).GetStream())
                    {
                        using (FileStream fileStream = new FileStream(path, FileMode.Open))
                        {
                            fileStream.CopyTo(zipStream);
                        }
                    }
                }
                else
                {

                }
            }
        }
    }

    private static void UnzipFilesFromStream(Stream source, string baseFolder, string zipFile = null, string baseFolderZip = null)
    {
        if (!Directory.Exists(baseFolder))
            Directory.CreateDirectory(baseFolder);

        using (Package package = Package.Open(source, FileMode.Open))
        {
            foreach (PackagePart zipPart in package.GetParts())
            {
                try
                {
                    // fix for white spaces in file names (by ErrCode)
                    var uri = Uri.UnescapeDataString(zipPart.Uri.ToString()).Substring(1);
                    if (zipFile.IsNotNull() && baseFolderZip.IsNotNull())
                        uri = UnzipHere(baseFolderZip, zipFile, zipPart);

                    string path = string.Format("{0}{1}", baseFolder, uri);

                    using (Stream zipStream = zipPart.GetStream())
                    {
                        CreateForders(path);
                        using (FileStream fileStream = new FileStream(path, FileMode.Create))
                        {
                            zipStream.CopyTo(fileStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(string.Format("Erro para descompactar {0}", zipPart.Uri.ToString()), ex);
                }
            }
        }
    }

    private static void CreateForders(string path)
    {
        var foldes = path.Replace(Path.GetFileName(path), String.Empty);
        if (!Directory.Exists(foldes))
            Directory.CreateDirectory(foldes);
    }

    private static string UnzipHere(string baseFolderZip, string zipFile, PackagePart zipPart)
    {
        var zipPath = zipPart.Uri.ToString().Replace("/", "\\");
        var baseFolderUnzip = string.Format("{0}\\", Path.GetFileNameWithoutExtension(zipFile));
        var uri = zipPath.Replace(baseFolderZip, baseFolderUnzip);
        return uri;
    }

    private static string GetFileExtentionName(string path)
    {
        string extention = Path.GetExtension(path);
        if (!string.IsNullOrWhiteSpace(extention) && extention.StartsWith("."))
        {
            extention = extention.Substring(1);
        }

        return extention;
    }
}