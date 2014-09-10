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

using System.Runtime.Serialization;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts
{
    /// <summary>
    /// This is a data contract class between extensions and resource provider
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespaces.Default)]
    public class StorageFile
    {
        [DataMember(Order = 1)]
        public int StorageFileId { get; set; }

        /// <summary>
        /// Name of the file.
        /// </summary>
        [DataMember(Order = 2)]
        public string StorageFileName { get; set; }

        /// <summary>
        /// Size of file. (KB) 
        /// </summary>
        [DataMember(Order = 3)]
        public int TotalSize { get; set; }
    }
}
