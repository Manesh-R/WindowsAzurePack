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
using System.IO;
using System.Linq;
using System.Web;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.DataProvider
{
    public class DataValidationUtil
    {
        internal static bool IsNetworkShareReachable(string path)
        {
            return Directory.Exists(path);
        }

        internal static bool IsNetworkAlreadyMapped(string path, List<Location> locations)
        {
            var existingLocation = (from s in locations where s.NetworkSharePath.ToLower() == path.ToLower() select s).FirstOrDefault();
            return existingLocation != null;
        }

        internal static bool IsLocationValid(Location location)
        {
            if (location == null ||
                location.TotalSpace <= 0 ||
                string.IsNullOrWhiteSpace(location.NetworkSharePath) ||
                string.IsNullOrWhiteSpace(location.LocationName))
            {
                return false;
            }

            return true;
        }
    }
}
