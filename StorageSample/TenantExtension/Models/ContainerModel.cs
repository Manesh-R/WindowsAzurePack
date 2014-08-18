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
    public class ContainerModel
    {
        public const string RegisteredStatus = "Registered";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerModel" /> class.
        /// </summary>
        public ContainerModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileServerModel" /> class.
        /// </summary>
        /// <param name="ProductModel">The domain name from API.</param>
        public ContainerModel(Container containerFromApi)
        {
            this.ContainerName = containerFromApi.ContainerName;
            this.SubscriptionId = containerFromApi.SubscriptionId;
            this.LocationId = containerFromApi.LocationId;
            this.URL = containerFromApi.URL;
            this.ContainerId = containerFromApi.ContainerId;
            this.id = containerFromApi.ContainerId;
        }

        /// <summary>
        /// Covert to the API object.
        /// </summary>
        /// <returns>The API DomainName data contract.</returns>
        public Container ToApiObject()
        {
            return new Container()
            {
                ContainerName = this.ContainerName,
                LocationId = this.LocationId,
                URL = this.URL,
                SubscriptionId = this.SubscriptionId,
                ContainerId = this.ContainerId
            };
        }

        /// <summary>
        /// Gets or sets the name.
        // </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or sets the value of the display name of the file server 
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the value of the subscription id
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the value of the container size
        /// </summary>
        public string URL { get; set; }

        public int ContainerId { get; set; }

        public int id { get; set; }
    }
}
