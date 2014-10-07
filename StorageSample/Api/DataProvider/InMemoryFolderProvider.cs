// ----------------------------------------------------------------------------------------------
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
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider
{
    /// <summary>
    /// Folder data provider with process memory as storage. If we restart IIS, information is lost.
    /// </summary>
    public class InMemoryFolderProvider : IFolderProvider
    {
        // We will start ID with 1000+, so that it looks good on UI.
        private static int CurrentMaxFolderId = 1000;

        private static InMemoryFolderProvider instance = new InMemoryFolderProvider();

        private static List<Folder> folders = new List<Folder>();

        public static IFolderProvider Instance
        {
            get { return instance;  }
        }

        List<Folder> IFolderProvider.GetFolders(string subscriptionId)
        {
            return (from folder in folders
                    where string.Equals(folder.SubscriptionId, subscriptionId, StringComparison.OrdinalIgnoreCase)
                    select folder).ToList();
        }

        Folder IFolderProvider.GetFolder(string subscriptionId, int folderId)
        {
            return folders.FirstOrDefault(c => c.FolderId == folderId && c.SubscriptionId == subscriptionId);
        }

        void IFolderProvider.CreateFolder(string subscriptionId, Folder folder)
        {
            folders.Add(new Folder
            {
                FolderId = CurrentMaxFolderId++,
                ShareId = folder.ShareId,
                FolderName = folder.FolderName,
                SubscriptionId = subscriptionId,
                URL = string.Format("{0}\\{1}\\{2}", 
                        InMemoryShareProvider.Instance.GetShares().First(l => l.ShareId == folder.ShareId).NetworkSharePath,
                        subscriptionId,
                        folder.FolderName)
            });
        }

        void IFolderProvider.UpdateFolder(string subscriptionId, Folder folderToUpdate)
        {
            var existing = (from c in folders
                             where c.FolderId == folderToUpdate.FolderId && string.Equals(c.SubscriptionId, folderToUpdate.SubscriptionId, StringComparison.OrdinalIgnoreCase)
                             select c).First();

            // Actually, we will not allow any updates to the folder properties for now.
            ////existing.FolderName = folderToUpdate.FolderName;
            ////existing.ShareId = folderToUpdate.ShareId;
            ////existing.URL = folderToUpdate.URL;
        }

        void IFolderProvider.DeleteFolder(string subscriptionId, int folderId)
        {
            folders.RemoveAll(c => c.FolderId == folderId && c.SubscriptionId == subscriptionId);
        }
    }
}
