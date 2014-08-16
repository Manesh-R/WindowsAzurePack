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
    public class ContainersController : ApiController
    {
        private static List<Container> containers = new List<Container>();

        public ContainersController()
        {
        }

        [HttpGet]
        public List<Container> ListContainers(string subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            return DataProviderFactory.ContainerInstance.GetContainers(subscriptionId);
        }

        [HttpPut]
        public void UpdateContainer(string subscriptionId, Container containerToUpdate)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            if (containerToUpdate == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerEmpty);
            }

            List<Container> containers = DataProviderFactory.ContainerInstance.GetContainers(subscriptionId);
            var existing = containers.FirstOrDefault(c => c.SubscriptionId == subscriptionId && c.ContainerId == containerToUpdate.ContainerId);
            if (existing == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerNotFound);
            }

            DataProviderFactory.ContainerInstance.UpdateContainer(subscriptionId, containerToUpdate);
        }

        [HttpPost]
        public void CreateContainer(string subscriptionId, Container container)
        {
            if (container == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerEmpty);
            }

            // Let us ensure that the same container name is not repeated in same locations for same subscription.
            List<Container> containers = DataProviderFactory.ContainerInstance.GetContainers(subscriptionId);
            var existing = containers.FirstOrDefault(c => c.SubscriptionId == subscriptionId &&
                                                    c.ContainerName.ToLower() == container.ContainerName.ToLower() &&
                                                    c.LocationId == container.LocationId);
            if (existing != null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerAlreadyExists);
            }

            // Invoke the provider so that data is presisted in store.
            DataProviderFactory.ContainerInstance.CreateContainer(subscriptionId, container);

            // Create the physical container directory.
            // TODO: If this fails, then we need to rollback changes made to container store DB.
            string dir = string.Format("{0}\\{1}\\{2}", 
                            DataProviderFactory.LocationInstance.GetLocations().First(l => l.LocationId == container.LocationId).NetworkSharePath,
                            subscriptionId,
                            container.ContainerName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        [HttpPost]
        public void DeleteContainer(string subscriptionId, Container container)
        {
            if (container == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerEmpty);
            }

            // Let us ensure that the same container name is not repeated in same locations for same subscription.
            List<Container> containers = DataProviderFactory.ContainerInstance.GetContainers(subscriptionId);
            var existing = containers.FirstOrDefault(c => c.SubscriptionId == subscriptionId && c.ContainerId == container.ContainerId);
            if (existing != null)
            {
                if (Directory.GetFiles(existing.URL).Length > 0)
                {
                    throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerNotEmpty);
                }

                Directory.Delete(existing.URL);
                DataProviderFactory.ContainerInstance.DeleteContainer(subscriptionId, container);
            }
        }
    }
}
