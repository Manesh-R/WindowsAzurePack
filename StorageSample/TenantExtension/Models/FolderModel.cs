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

using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.TenantExtension.Models
{
    /// <summary>
    /// Data model for domain name tenant view
    /// </summary>    
    public class FolderModel
    {
        public const string RegisteredStatus = "Registered";

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderModel" /> class.
        /// </summary>
        public FolderModel()
        {
            this.Type = "Folder";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileServerModel" /> class.
        /// </summary>
        /// <param name="ProductModel">The domain name from API.</param>
        public FolderModel(Folder folderFromApi)
        {
            this.FolderName = folderFromApi.FolderName;
            this.SubscriptionId = folderFromApi.SubscriptionId;
            this.ShareId = folderFromApi.ShareId;
            this.URL = folderFromApi.URL;
            this.FolderId = folderFromApi.FolderId;
            this.id = folderFromApi.FolderId;
            this.Type = "Folder";
        }

        /// <summary>
        /// Covert to the API object.
        /// </summary>
        /// <returns>The API DomainName data contract.</returns>
        public Folder ToApiObject()
        {
            return new Folder()
            {
                FolderName = this.FolderName,
                ShareId = this.ShareId,
                URL = this.URL,
                SubscriptionId = this.SubscriptionId,
                FolderId = this.FolderId
            };
        }

        /// <summary>
        /// Gets or sets the name.
        // </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the value of the display name of the file server 
        /// </summary>
        public int ShareId { get; set; }

        /// <summary>
        /// Gets or sets the value of the subscription id
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the value of the folder size
        /// </summary>
        public string URL { get; set; }

        public int FolderId { get; set; }

        public int id { get; set; }

        /// <summary>
        /// This property is required and should be set correct so that navigation to sub-tab will work properly.
        /// </summary>
        public string Type { get; set; }
    }
}
