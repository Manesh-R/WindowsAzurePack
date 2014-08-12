// ---------------------------------------------------------
// Copyright (c) Terawe Corporation. All rights reserved.
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
// ---------------------------------------------------------

using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.TenantExtension.Models
{
    /// <summary>
    /// Data model for domain name tenant view
    /// </summary>    
    public class LocationModel
    {
        public const string RegisteredStatus = "Registered";

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationModel" /> class.
        /// </summary>
        public LocationModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationModel" /> class.
        /// </summary>
        /// <param name="ProductModel">The domain name from API.</param>
        public LocationModel(Location locationFromApi)
        {
            this.LocationId = locationFromApi.LocationId;
            this.LocationName = locationFromApi.LocationName;
        }

        /// <summary>
        /// Covert to the API object.
        /// </summary>
        /// <returns>The API DomainName data contract.</returns>
        public Location ToApiObject()
        {
            return new Location()
            {
                LocationId = this.LocationId,
                LocationName = this.LocationName
            };
        }

        /// <summary>
        /// Gets or sets the name.
        // </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the value of the display name of the file server 
        /// </summary>
        public string LocationName { get; set; }
    }
}
