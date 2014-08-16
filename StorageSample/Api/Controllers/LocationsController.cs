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
using System.Web.Http;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.Controllers
{
    public class LocationsController : ApiController
    {
        [HttpGet]
        public List<Location> GetLocationList(string subscriptionId = null)
        {
            return DataProviderFactory.LocationInstance.GetLocations();
        }

        [HttpPut]
        public void UpdateLocation(Location location)
        {
            if (location == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.LocationEmpty);
            }

            // TODO: Fix issue around HTTP POST method.
            if (location.LocationId == 0)
            {
                // Treat this as Add Location
                InMemoryLocationProvider.Instance.CreateLocation(location);
                return;
            }

            var locations = DataProviderFactory.LocationInstance.GetLocations();
            var existingLocation = (from s in locations where s.LocationId == location.LocationId select s).FirstOrDefault();

            if (existingLocation == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.LocationNotFound, location.LocationName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            DataProviderFactory.LocationInstance.UpdateLocation(location);
        }

        [HttpPost]
        public void AddLocation(Location location)
        {
            if (location == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.LocationEmpty);
            }

            if (!DataValidationUtil.IsLocationValid(location))
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.NullInput);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            var locations = DataProviderFactory.LocationInstance.GetLocations();
            var existingLocation = (from s in locations where s.LocationName.ToLower() == location.LocationName.ToLower() select s).FirstOrDefault();
            if (existingLocation != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.LocationAlreadyExist, location.LocationName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            };

            if (!DataValidationUtil.IsNetworkShareReachable(location.NetworkSharePath))
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.LocationNotFound, location.NetworkSharePath);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            // Trim trailing slash and space from path.
            location.NetworkSharePath = location.NetworkSharePath.TrimEnd(new char[] { ' ', '\\' });
            if (DataValidationUtil.IsNetworkAlreadyMapped(location.NetworkSharePath, locations))
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.NetworkShareAlreadyMapped, location.NetworkSharePath);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            DataProviderFactory.LocationInstance.CreateLocation(location);
        }

        [HttpPost]
        public void DeleteLocation(Location location)
        {
            if (location == null || location.LocationId == 0)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.LocationEmpty);
            }

            var locations = DataProviderFactory.LocationInstance.GetLocations();
            var existingLocation = (from s in locations where s.LocationId == location.LocationId select s).FirstOrDefault();

            if (existingLocation == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.LocationNotFound, location.LocationName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            DataProviderFactory.LocationInstance.DeleteLocation(location);
        }
    }
}
