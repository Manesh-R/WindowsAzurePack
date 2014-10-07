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
    public class ShareModel
    {
        public const string RegisteredStatus = "Registered";

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareModel" /> class.
        /// </summary>
        public ShareModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareModel" /> class.
        /// </summary>
        /// <param name="ProductModel">The domain name from API.</param>
        public ShareModel(Share locationFromApi)
        {
            this.ShareId = locationFromApi.ShareId;
            this.ShareName = locationFromApi.ShareName;
        }

        /// <summary>
        /// Covert to the API object.
        /// </summary>
        /// <returns>The API DomainName data contract.</returns>
        public Share ToApiObject()
        {
            return new Share()
            {
                ShareId = this.ShareId,
                ShareName = this.ShareName
            };
        }

        /// <summary>
        /// Gets or sets the name.
        // </summary>
        public int ShareId { get; set; }

        /// <summary>
        /// Gets or sets the value of the display name of the file server 
        /// </summary>
        public string ShareName { get; set; }
    }
}
