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
    /// <summary>
    /// Container data provider with process memory as storage. If we restart IIS, information is lost.
    /// </summary>
    public class InMemoryContainerProvider : IContainerProvider
    {
        // We will start ID with 1000+, so that it looks good on UI.
        private static int CurrentMaxContainerId = 1000;

        private static InMemoryContainerProvider instance = new InMemoryContainerProvider();

        private static List<Container> containers = new List<Container>();

        public static IContainerProvider Instance
        {
            get { return instance;  }
        }

        List<Container> IContainerProvider.GetContainers(string subscriptionId)
        {
            return (from container in containers
                    where string.Equals(container.SubscriptionId, subscriptionId, StringComparison.OrdinalIgnoreCase)
                    select container).ToList();
        }

        Container IContainerProvider.GetContainer(string subscriptionId, int containerId)
        {
            return containers.FirstOrDefault(c => c.ContainerId == containerId && c.SubscriptionId == subscriptionId);
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
            var existing = (from c in containers
                             where c.ContainerId == containerToUpdate.ContainerId && string.Equals(c.SubscriptionId, containerToUpdate.SubscriptionId, StringComparison.OrdinalIgnoreCase)
                             select c).First();

            // Actually, we will not allow any updates to the container properties for now.
            ////existing.ContainerName = containerToUpdate.ContainerName;
            ////existing.LocationId = containerToUpdate.LocationId;
            ////existing.URL = containerToUpdate.URL;
        }

        void IContainerProvider.DeleteContainer(string subscriptionId, int containerId)
        {
            containers.RemoveAll(c => c.ContainerId == containerId && c.SubscriptionId == subscriptionId);
        }
    }
}
