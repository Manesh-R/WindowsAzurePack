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
    public class StorageFilesController : ApiController
    {
        public StorageFilesController()
        {
        }

        [HttpGet]
        public List<StorageFile> ListContainers(string subscriptionId = null, int containerId = 0)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            if (containerId <= 0)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidContainerId);
            }

            // This is for demonstrating the sub-navigation. So returning dummy data always.
            List<StorageFile> files = new List<StorageFile>();
            if (containerId % 2 == 0)
            {
                files.Add(new StorageFile() { StorageFileId = 2001, StorageFileName = "Education.png", TotalSize = 450 });
                files.Add(new StorageFile() { StorageFileId = 2002, StorageFileName = "Students.png", TotalSize = 150 });
                files.Add(new StorageFile() { StorageFileId = 2003, StorageFileName = "Lit4Life.png", TotalSize = 1090 });
            }
            return files;
        }
    }
}
