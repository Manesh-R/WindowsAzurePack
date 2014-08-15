// ---------------------------------------------------------------------------
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
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider
{
    public class InMemoryLocationProvider : ILocationProvider
    {
        private static InMemoryLocationProvider instance = new InMemoryLocationProvider();

        private static int CurrentMaxLocationId = 1000;

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
            var existingLocation = (from s in locations where s.LocationName == location.LocationName select s).FirstOrDefault();

            if (existingLocation != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.LocationAlreadyExist, location.LocationName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            };

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
            // TODO: Fix issue around HTTP POST method.
            if (location.LocationId == 0)
            {
                // Treat this as Add Location
                InMemoryLocationProvider.Instance.CreateLocation(location);
                return;
            }

            var existingLocation = (from s in locations where s.LocationId == location.LocationId select s).FirstOrDefault();

            if (existingLocation == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.LocationNotFound, location.LocationName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }
            else
            {
                existingLocation.LocationName = location.LocationName;
                existingLocation.TotalSpace = location.TotalSpace;
                existingLocation.FreeSpace = location.FreeSpace;
                existingLocation.NetworkSharePath = location.NetworkSharePath;
            }
        }

        void ILocationProvider.DeleteLocation(Location location)
        {
            var existingLocation = (from s in locations where s.LocationId == location.LocationId select s).FirstOrDefault();

            if (existingLocation != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.LocationNotFound, location.LocationName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }
            else
            {
                locations.Remove(existingLocation);
            }
        }
    }
}
