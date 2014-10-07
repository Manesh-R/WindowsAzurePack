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
    public class FoldersController : ApiController
    {
        private static List<Folder> folders = new List<Folder>();

        public FoldersController()
        {
        }

        [HttpGet]
        public List<Folder> ListFolders(string subscriptionId = null)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            return DataProviderFactory.FolderInstance.GetFolders(subscriptionId);
        }

        [HttpPut]
        public void UpdateFolder(string subscriptionId, Folder folderToUpdate)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            if (folderToUpdate == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FolderEmpty);
            }

            List<Folder> folders = DataProviderFactory.FolderInstance.GetFolders(subscriptionId);
            var existing = folders.FirstOrDefault(c => c.SubscriptionId == subscriptionId && c.FolderId == folderToUpdate.FolderId);
            if (existing == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FolderNotFound);
            }

            DataProviderFactory.FolderInstance.UpdateFolder(subscriptionId, folderToUpdate);
        }

        [HttpPost]
        public void CreateFolder(string subscriptionId, Folder folder)
        {
            if (folder == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FolderEmpty);
            }

            // Let us ensure that the same folder name is not repeated in same shares for same subscription.
            List<Folder> folders = DataProviderFactory.FolderInstance.GetFolders(subscriptionId);
            var existing = folders.FirstOrDefault(c => c.SubscriptionId == subscriptionId &&
                                                    c.FolderName.ToLower() == folder.FolderName.ToLower() &&
                                                    c.ShareId == folder.ShareId);
            if (existing != null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FolderAlreadyExists);
            }

            // Invoke the provider so that data is presisted in store.
            DataProviderFactory.FolderInstance.CreateFolder(subscriptionId, folder);

            // Create the physical folder directory.
            // TODO: If this fails, then we need to rollback changes made to folder store DB.
            string dir = string.Format("{0}\\{1}\\{2}", 
                            DataProviderFactory.ShareInstance.GetShares().First(l => l.ShareId == folder.ShareId).NetworkSharePath,
                            subscriptionId,
                            folder.FolderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        [HttpDelete]
        public void DeleteFolder(string subscriptionId, int folderId)
        {
            if (folderId == 0)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FolderEmpty);
            }

            // Let us ensure that the same folder name is not repeated in same shares for same subscription.
            List<Folder> folders = DataProviderFactory.FolderInstance.GetFolders(subscriptionId);
            var existing = folders.FirstOrDefault(c => c.SubscriptionId == subscriptionId && c.FolderId == folderId);
            if (existing != null)
            {
                if (Directory.GetFiles(existing.URL).Length > 0)
                {
                    throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.FolderNotEmpty);
                }

                Directory.Delete(existing.URL);
                DataProviderFactory.FolderInstance.DeleteFolder(subscriptionId, folderId);
            }
        }
    }
}
