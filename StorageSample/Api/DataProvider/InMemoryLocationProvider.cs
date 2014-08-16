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
using System.IO;
using System.Linq;
using System.Web;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider
{
    /// <summary>
    /// Location data provider with process memory as storage. If we restart IIS, information is lost.
    /// </summary>
    public class InMemoryLocationProvider : ILocationProvider
    {
        // We will start ID with 1000+, so that it looks good on UI.
        private static int CurrentMaxLocationId = 1000;

        private static InMemoryLocationProvider instance = new InMemoryLocationProvider();

        public static List<Location> locations = new List<Location>();

        public static ILocationProvider Instance
        {
            get { return instance; }
        }

        List<Location> ILocationProvider.GetLocations(string subscriptionId)
        {
            return locations;
        }

        void ILocationProvider.CreateLocation(Location location)
        {
            CurrentMaxLocationId++;
            locations.Add(new Location
            {
                LocationId = CurrentMaxLocationId,
                LocationName = location.LocationName,
                TotalSpace = location.TotalSpace,
                FreeSpace = location.TotalSpace,   // When we start, all space is free.
                NetworkSharePath = location.NetworkSharePath
            });
        }

        void ILocationProvider.UpdateLocation(Location location)
        {
            var existingLocation = (from s in locations where s.LocationId == location.LocationId select s).First();
            existingLocation.TotalSpace = location.TotalSpace;
            existingLocation.FreeSpace = location.FreeSpace;

            // For now, we only allow updating free and total space. 
            // We do not allow the location name or path to be edited, as files will be already written there.
            ////existingLocation.LocationName = location.LocationName;
            ////existingLocation.NetworkSharePath = location.NetworkSharePath;
        }

        void ILocationProvider.DeleteLocation(Location location)
        {
            // Call will come here only if the location is valid.
            var existingLocation = (from s in locations where s.LocationId == location.LocationId select s).First();
            if (existingLocation != null)
            {
                locations.Remove(existingLocation);
            }
        }
    }
}
