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

using System.Runtime.Serialization;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts
{
    /// <summary>
    /// This is a data contract class between extensions and resource provider
    /// Location contains data contract of data which shows up in "Locations" tab inside StorageSample Admin Extension
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespaces.Default)]
    public class Location
    {
        [DataMember(Order = 1)]
        public int LocationId { get; set; }

        /// <summary>
        /// Name of the file server
        /// </summary>
        [DataMember(Order = 2)]
        public string LocationName { get; set; }

        /// <summary>
        /// Total space in File Server (MB) 
        /// </summary>
        [DataMember(Order = 3)]
        public int TotalSpace { get; set; }

        /// <summary>
        /// Total Free Space available in file server (MB)
        /// </summary>
        [DataMember(Order = 4)]
        public int FreeSpace { get; set; }


        /// <summary>
        /// Network fileshare path.
        /// </summary>
        [DataMember(Order = 5)]
        public string NetworkSharePath { get; set; }
    }
}
