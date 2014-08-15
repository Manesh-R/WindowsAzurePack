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

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.AdminExtension.Models
{
    /// <summary>
    /// This is a model class which contains data contract we send to Controller which then shows up in UI
    /// LocationModel contains data contract of data which shows up in "File Servers" tab inside StorageSample Admin Extension
    /// </summary>
    public class LocationModel
    {
        /// <summary>
        /// This is hidden in UI but can be used to identify individual file server records in grid
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// LocationName maps to "Name" column in "File Servers" tab
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// TotalSpace maps to "Total Space" column in "File Servers" tab
        /// Specify space in GB
        /// </summary>
        public int TotalSpace { get; set; }

        /// <summary>
        /// FreeSpace maps to "Free Space" column in "File Servers" tab        
        /// </summary>
        public int FreeSpace { get; set; }

        /// <summary>
        /// DefaultSize maps to "Default Share Size" column in "File Servers" tab
        /// </summary>
        public string NetworkSharePath { get; set; }

        public LocationModel(Location location)
        {
            this.LocationId = location.LocationId;
            this.LocationName = location.LocationName;
            this.TotalSpace = location.TotalSpace;
            this.FreeSpace = location.FreeSpace;
            this.NetworkSharePath = location.NetworkSharePath;
        }
    }
}
