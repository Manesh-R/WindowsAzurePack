// ----------------------------------------------------------------------------
// Copyright (c) Terawe Corporation.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Http;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;
using IO = System.IO;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.Controllers
{
    public class StorageFilesController : ApiController
    {
        public StorageFilesController()
        {
        }

        [HttpGet]
        public List<StorageFile> ListFolders(string subscriptionId = null, int folderId = 0)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            if (folderId <= 0)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidFolderId);
            }

            // This is for demonstrating the sub-navigation. So returning dummy data always.
            
            Folder folder = DataProviderFactory.FolderInstance.GetFolder(subscriptionId, folderId);
            string[] files = Directory.GetFiles(folder.URL);

            List<StorageFile> storageFiles = new List<StorageFile>();
            foreach(string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                storageFiles.Add(new StorageFile() { StorageFileName = fileInfo.Name, TotalSize = fileInfo.Length });
            }

            return storageFiles;
        }

        [HttpPost]
        public void CreateFile(string subscriptionId, int folderId, StorageFile storageFile)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            if (folderId <= 0)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidFolderId);
            }

            Folder folder = DataProviderFactory.FolderInstance.GetFolder(subscriptionId, folderId);
            string filePath = string.Format(@"{0}\{1}", folder.URL, storageFile.StorageFileName);
            File.WriteAllBytes(filePath, storageFile.FileContent);
        }
    }
}
