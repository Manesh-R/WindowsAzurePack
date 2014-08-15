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
using System.Linq;
using System.Web.Http;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;
using IO = System.IO;
using System.Globalization;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.Controllers
{
    public class ContainersController : ApiController
    {
        private static List<Container> containers = new List<Container>();

        [HttpGet]
        public List<Container> ListContainers(string subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentNullException(subscriptionId);
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

            DataProviderFactory.ContainerInstance.UpdateContainer(subscriptionId, containerToUpdate);
        }

        [HttpPost]
        public void CreateContainer(string subscriptionId, Container container)
        {
            if (container == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerEmpty);
            }

            DataProviderFactory.ContainerInstance.CreateContainer(subscriptionId, container);
        }
    }
}
