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
    public class SharesController : ApiController
    {
        [HttpGet]
        public List<Share> GetShareList(string subscriptionId = null)
        {
            return DataProviderFactory.ShareInstance.GetShares();
        }

        [HttpPut]
        public void UpdateShare(Share location)
        {
            if (location == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ShareEmpty);
            }

            // TODO: Fix issue around HTTP POST method.
            if (location.ShareId == 0)
            {
                // Treat this as Add Share
                InMemoryShareProvider.Instance.CreateShare(location);
                return;
            }

            var locations = DataProviderFactory.ShareInstance.GetShares();
            var existingShare = (from s in locations where s.ShareId == location.ShareId select s).FirstOrDefault();

            if (existingShare == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.ShareNotFound, location.ShareName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            DataProviderFactory.ShareInstance.UpdateShare(location);
        }

        [HttpPost]
        public void AddShare(Share location)
        {
            if (location == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ShareEmpty);
            }

            if (!DataValidationUtil.IsShareValid(location))
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.NullInput);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            var locations = DataProviderFactory.ShareInstance.GetShares();
            var existingShare = (from s in locations where s.ShareName.ToLower() == location.ShareName.ToLower() select s).FirstOrDefault();
            if (existingShare != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.ShareAlreadyExist, location.ShareName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            };

            if (!DataValidationUtil.IsNetworkShareReachable(location.NetworkSharePath))
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.ShareNotFound, location.NetworkSharePath);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            // Trim trailing slash and space from path.
            location.NetworkSharePath = location.NetworkSharePath.TrimEnd(new char[] { ' ', '\\' });
            if (DataValidationUtil.IsNetworkAlreadyMapped(location.NetworkSharePath, locations))
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.NetworkShareAlreadyMapped, location.NetworkSharePath);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            DataProviderFactory.ShareInstance.CreateShare(location);
        }

        [HttpPost]
        public void DeleteShare(Share location)
        {
            if (location == null || location.ShareId == 0)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.ShareEmpty);
            }

            var locations = DataProviderFactory.ShareInstance.GetShares();
            var existingShare = (from s in locations where s.ShareId == location.ShareId select s).FirstOrDefault();

            if (existingShare == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.ShareNotFound, location.ShareName);
                throw Utility.ThrowResponseException(null, System.Net.HttpStatusCode.BadRequest, message);
            }

            DataProviderFactory.ShareInstance.DeleteShare(location);
        }
    }
}
