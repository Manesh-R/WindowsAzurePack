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
    /// Share data provider with process memory as storage. If we restart IIS, information is lost.
    /// </summary>
    public class InMemoryShareProvider : IShareProvider
    {
        // We will start ID with 1000+, so that it looks good on UI.
        private static int CurrentMaxShareId = 1000;

        private static InMemoryShareProvider instance = new InMemoryShareProvider();

        public static List<Share> locations = new List<Share>();

        public static IShareProvider Instance
        {
            get { return instance; }
        }

        List<Share> IShareProvider.GetShares(string subscriptionId)
        {
            return locations;
        }

        void IShareProvider.CreateShare(Share location)
        {
            CurrentMaxShareId++;
            locations.Add(new Share
            {
                ShareId = CurrentMaxShareId,
                ShareName = location.ShareName,
                TotalSpace = location.TotalSpace,
                FreeSpace = location.TotalSpace,   // When we start, all space is free.
                NetworkSharePath = location.NetworkSharePath
            });
        }

        void IShareProvider.UpdateShare(Share location)
        {
            var existingShare = (from s in locations where s.ShareId == location.ShareId select s).First();
            existingShare.TotalSpace = location.TotalSpace;
            existingShare.FreeSpace = location.FreeSpace;

            // For now, we only allow updating free and total space. 
            // We do not allow the location name or path to be edited, as files will be already written there.
            ////existingShare.ShareName = location.ShareName;
            ////existingShare.NetworkSharePath = location.NetworkSharePath;
        }

        void IShareProvider.DeleteShare(Share location)
        {
            // Call will come here only if the location is valid.
            var existingShare = (from s in locations where s.ShareId == location.ShareId select s).First();
            if (existingShare != null)
            {
                locations.Remove(existingShare);
            }
        }
    }
}
