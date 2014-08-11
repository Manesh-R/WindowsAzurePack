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
    /// FileServer contains data contract of data which shows up in "File Shares" tab inside StorageSample Tenant Extension
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespaces.Default)]
    public class Container
    {
        /// <summary>
        /// Name of the file share
        /// </summary>
        [DataMember(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// SubscriptionId of user who created this file share
        /// </summary>
        [DataMember(Order = 2)]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Name of the file server where file share resides
        /// </summary>
        [DataMember(Order = 3)]
        public string FileServerName { get; set; }

        /// <summary>
        /// Size of the file share
        /// </summary>
        [DataMember(Order = 4)]
        public int Size { get; set; }

        /// <summary>
        /// Id of the file share
        /// </summary>
        [DataMember(Order = 5)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the extension data.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
