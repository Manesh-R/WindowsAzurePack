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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.WindowsAzurePack.Samples.HelloWorld.Common;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.TenantExtension.Models;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.TenantExtension.Controllers
{
    [RequireHttps]
    [OutputCache(Location = OutputCacheLocation.None)]
    [PortalExceptionHandler]
    public sealed class StorageSampleTenantController : ExtensionController
    {   
        /// <summary>
        /// List containers belong to subscription
        /// NOTE: For this sample dummy entries will be displayed
        /// </summary>
        /// <param name="subscriptionIds"></param>
        /// <returns></returns>
        [HttpPost]        
        public async Task<JsonResult> ListContainers(string[] subscriptionIds)
        {
            // Make the requests sequentially for simplicity
            var containers = new List<ContainerModel>();

            if (subscriptionIds == null || subscriptionIds.Length == 0)
            {
                throw new HttpException("Subscription Id not found");
            }

            foreach (var subId in subscriptionIds)
            {
                var containersFromApi = await ClientFactory.StorageSampleClient.ListContainersAsync(subId);
                containers.AddRange(containersFromApi.Select(d => new ContainerModel(d)));
            }

            return this.JsonDataSet(containers);
        }

        /// <summary>
        /// List containers belong to subscription
        /// NOTE: For this sample dummy entries will be displayed
        /// </summary>
        /// <param name="subscriptionIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ListLocations(string subscriptionIds)
        {
            // Make the requests sequentially for simplicity
            var locations = new List<LocationModel>();

            var locationsFromApi = await ClientFactory.StorageSampleClient.GetLocationListForTenantAsync(subscriptionIds);
            locations.AddRange(locationsFromApi.Select(l => new LocationModel(l)));

            return this.JsonDataSet(locations);
        }

        /// <summary>
        /// Create new container for subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="containerToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult>  CreateContainer(string subscriptionId, ContainerModel container)
        {
            await ClientFactory.StorageSampleClient.CreateContainerAsync(subscriptionId, container.ToApiObject());
            return this.Json(container);
        }

        /// <summary>
        /// Create new container for subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="containerToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteContainer(string subscriptionId, int containerId)
        {
            await ClientFactory.StorageSampleClient.DeleteContainerAsync(subscriptionId, containerId);
            return this.Json(new object());
        }

        /// <summary>
        /// List files in a specific container.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ListStorageFiles(string subscriptionId, int containerId)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new HttpException("Subscription Id is not valid");
            }

            var files = await ClientFactory.StorageSampleClient.GetFileListForTenantAsync(subscriptionId, containerId);
            return this.JsonDataSet(files);
        }

        /// <summary>
        /// Upload file to a storage folder.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadStorageFile(HttpPostedFileWrapper uploadImageFile, string subscriptionId, int containerId)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new HttpException("Subscription Id is not valid");
            }

            if (uploadImageFile == null || uploadImageFile.ContentLength == 0)
            {
                throw new HttpException("Uploaded file is not valid");
            }

            string fileName = uploadImageFile.FileName.Substring(uploadImageFile.FileName.LastIndexOf('\\') + 1);

            // Get uploaded file content.
            System.IO.Stream inStream = uploadImageFile.InputStream;
            byte[] fileData = new byte[uploadImageFile.ContentLength];
            inStream.Read(fileData, 0, uploadImageFile.ContentLength);


            await ClientFactory.StorageSampleClient.UploadForTenantAsync(subscriptionId, containerId, fileName, fileData);
            return this.JsonDataSet(new object());
        }
    }
}
