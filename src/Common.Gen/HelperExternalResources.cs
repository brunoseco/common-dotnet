using Common.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace Common.Gen
{

    public class ExternalResource
    {

        public ExternalResource()
        {
            this.ReplaceLocalFilesApplication = true;
            this.DownloadOneTime = false;
            this.DownloadOneTimeFileVerify = string.Empty;
        }

        public bool ReplaceLocalFilesApplication { get; set; }
        public bool DownloadOneTime { get; set; }
        public string DownloadOneTimeFileVerify { get; set; }

        public string ResouceRepositoryName { get; set; }
        public string ResourceUrlRepository { get; set; }
        public string ResourceLocalPathFolderExecuteCloning { get; set; }
        public string ResourceLocalPathDestinationFolrderApplication { get; set; }
        public string OnlyFoldersContainsThisName { get; set; }

        public IEnumerable<string> OnlyThisFiles { get; set; }

        public string ResourceLocalPathFolderCloningRepository
        {
            get
            {
                return string.Format("{0}\\{1}", this.ResourceLocalPathFolderExecuteCloning, this.ResouceRepositoryName);
            }
        }


    }

    public static class HelperExternalResources
    {

        public static void CloneAndCopy(IEnumerable<ExternalResource> resources)
        {
            foreach (var resource in resources)
            {
                if (!ContinueFlow(resource))
                    continue;

                clone(resource);

                if (resource.OnlyThisFiles.IsAny())
                {
                    foreach (var item in resource.OnlyThisFiles)
                    {
                        var pathSource = Path.Combine(resource.ResourceLocalPathFolderExecuteCloning, resource.ResouceRepositoryName, item);
                        var fileSource = new FileInfo(pathSource);
                        var fileDestination = Path.Combine(resource.ResourceLocalPathDestinationFolrderApplication, item);

                        var directoryBaseDestination = Path.GetDirectoryName(fileDestination);
                        if (!Directory.Exists(directoryBaseDestination))
                            Directory.CreateDirectory(directoryBaseDestination);

                        fileSource.CopyTo(fileDestination, true);
                    }

                }
                else if (resource.ReplaceLocalFilesApplication)
                    HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1}  /s /e /xd *\"bin\" *\"obj\" *\".git\" /xf *\".sln\" *\".md\" ", resource.ResourceLocalPathFolderCloningRepository, resource.ResourceLocalPathDestinationFolrderApplication), 10000);

            }

        }


        public static void CloneOnly(IEnumerable<ExternalResource> resources)
        {
            foreach (var resource in resources)
            {
                clone(resource);
            }

        }

        public static void UpdateLocalRepository(IEnumerable<ExternalResource> resources)
        {
            foreach (var resource in resources)
            {

                if (!ContinueFlow(resource))
                    continue;

                if (resource.OnlyFoldersContainsThisName.IsNotNullOrEmpty())
                {
                    var foldersActual = new DirectoryInfo(resource.ResourceLocalPathDestinationFolrderApplication).GetDirectories()
                            .Where(_ => _.Name.Contains(resource.OnlyFoldersContainsThisName));

                    foreach (var folderActual in foldersActual)
                    {
                        HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e /xd *\"bin\" *\"obj\"", folderActual.FullName, string.Format("{0}\\{1}", resource.ResourceLocalPathFolderCloningRepository, folderActual.Name)), 10000);
                    }
                }
                else if (resource.OnlyThisFiles.IsAny())
                {
                    foreach (var item in resource.OnlyThisFiles)
                    {
                        var pathSource = Path.Combine(resource.ResourceLocalPathDestinationFolrderApplication, item);
                        var fileSource = new FileInfo(pathSource);
                        var fileDestination = Path.Combine(resource.ResourceLocalPathFolderExecuteCloning, resource.ResouceRepositoryName, item);

                        var directoryBaseDestination = Path.GetDirectoryName(fileDestination);
                        if (!Directory.Exists(directoryBaseDestination))
                            Directory.CreateDirectory(directoryBaseDestination);

                        fileSource.CopyTo(fileDestination,true);
                    }
                }
                else
                {
                    HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e", resource.ResourceLocalPathDestinationFolrderApplication, resource.ResourceLocalPathFolderCloningRepository), 10000);
                }

            }

        }

        # region helper

        private static void clone(ExternalResource resource)
        {
            if (Directory.Exists(resource.ResourceLocalPathFolderCloningRepository))
                HelperCmd.ExecuteCommand(string.Format("RMDIR {0} /S /Q", resource.ResourceLocalPathFolderCloningRepository), 10000);

            HelperCmd.ExecuteCommand(string.Format("git clone {0} {1}", resource.ResourceUrlRepository, resource.ResourceLocalPathFolderCloningRepository), 10000);
        }

        private static void Bkp(ExternalResource resource)
        {
            var bkpPathFolder = string.Format("{0}\\{1}-BKP", AppDomain.CurrentDomain.BaseDirectory, resource.ResouceRepositoryName);

            if (resource.OnlyFoldersContainsThisName.IsNotNullOrEmpty())
            {
                var foldersActual = new DirectoryInfo(resource.ResourceLocalPathDestinationFolrderApplication).GetDirectories()
                    .Where(_ => _.Name.Contains(resource.OnlyFoldersContainsThisName));

                foreach (var folderActual in foldersActual)
                {
                    if (Directory.Exists(folderActual.FullName))
                        HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e /xd *\"bin\" *\"obj\"", folderActual.FullName, string.Format("{0}\\{1}", bkpPathFolder, folderActual.Name)), 10000);
                }
            }
            else
            {
                if (Directory.Exists(resource.ResourceLocalPathDestinationFolrderApplication))
                    HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e", resource.ResourceLocalPathDestinationFolrderApplication, bkpPathFolder), 10000);
            }
        }

        private static bool ContinueFlow(ExternalResource resource)
        {
            var continueFlow = true;
            if (resource.DownloadOneTime)
            {
                var fileVerify = string.Format("{0}\\{1}", resource.ResourceLocalPathDestinationFolrderApplication, resource.DownloadOneTimeFileVerify);
                if (File.Exists(fileVerify))
                    continueFlow = false;
            }

            return continueFlow;
        }

        #endregion

    }
}
