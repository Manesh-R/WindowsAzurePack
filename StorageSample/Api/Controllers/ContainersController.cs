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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;
using IO = System.IO;
using System.Globalization;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.Controllers
{
    public class ContainersController : ApiController
    {
        public static List<Container> containers = new List<Container>();

        [HttpGet]
        public List<Container> ListContainers(string subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentNullException(subscriptionId);
            }

            var shares = from share in containers
                         where string.Equals(share.SubscriptionId, subscriptionId, StringComparison.OrdinalIgnoreCase)
                         select share;

            return shares.ToList();
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

            var container = (from share in containers
                             where share.ContainerId == containerToUpdate.ContainerId && string.Equals(share.SubscriptionId, containerToUpdate.SubscriptionId, StringComparison.OrdinalIgnoreCase)
                             select share).FirstOrDefault();

            if (container != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.ContainerNotFound, container.ContainerName);
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, message);
            }

            container.ContainerName = containerToUpdate.ContainerName;
            container.LocationId = containerToUpdate.LocationId;
            container.URL = containerToUpdate.URL;
        }

        [HttpPost]
        public void CreateContainer(string subscriptionId, Container container)
        {
            if (container == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ContainerEmpty);
            }

            containers.Add(new Container
            {
                ContainerId = containers.Count,
                LocationId = container.LocationId,
                ContainerName = container.ContainerName,
                SubscriptionId = subscriptionId,
                URL = container.URL
            });
        }
    }
}
