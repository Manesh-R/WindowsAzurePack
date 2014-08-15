// ----------------------------------------------------------------------------------------------
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
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider
{
    public class InMemoryContainerProvider : IContainerProvider
    {
        private static InMemoryContainerProvider instance = new InMemoryContainerProvider();

        private static int CurrentMaxContainerId = 1000;

        private static List<Container> containers = new List<Container>();

        public static IContainerProvider Instance
        {
            get { return instance;  }
        }

        List<Container> IContainerProvider.GetContainers(string subscriptionId)
        {
            var shares = from share in containers
                         where string.Equals(share.SubscriptionId, subscriptionId, StringComparison.OrdinalIgnoreCase)
                         select share;

            return shares.ToList();
        }

        void IContainerProvider.CreateContainer(string subscriptionId, Container container)
        {
            containers.Add(new Container
            {
                ContainerId = CurrentMaxContainerId++,
                LocationId = container.LocationId,
                ContainerName = container.ContainerName,
                SubscriptionId = subscriptionId,
                URL = string.Format("{0}\\{1}\\{2}", 
                        InMemoryLocationProvider.Instance.GetLocations().First(l => l.LocationId == container.LocationId).NetworkSharePath,
                        subscriptionId,
                        container.ContainerName)
            });
        }

        void IContainerProvider.UpdateContainer(string subscriptionId, Container containerToUpdate)
        {
            var container = (from share in containers
                             where share.ContainerId == containerToUpdate.ContainerId && string.Equals(share.SubscriptionId, containerToUpdate.SubscriptionId, StringComparison.OrdinalIgnoreCase)
                             select share).FirstOrDefault();

            if (container != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.ContainerNotFound, container.ContainerName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            container.ContainerName = containerToUpdate.ContainerName;
            container.LocationId = containerToUpdate.LocationId;
            container.URL = containerToUpdate.URL;
        }

        void IContainerProvider.DeleteContainer(string subscriptionId, Container container)
        {
            throw new NotImplementedException();
        }
    }
}
