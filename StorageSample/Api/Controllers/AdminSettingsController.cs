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
using System.Web.Http;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.Controllers
{
    public class AdminSettingsController : ApiController
    {
        public static AdminSettings settings;

        static AdminSettingsController()
        {
            settings = new AdminSettings
            {
                EndpointAddress = "http://myserver:300",
                Username = "adminUser",
                Password = "adminPwd"
            };
        }

        [HttpGet]
        public AdminSettings GetAdminSettings()
        {
           return settings;
        }

        [HttpPut]
        public void UpdateAdminSettings(AdminSettings newSettings)
        {
            if (newSettings == null)
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.NullInput); 
            }

            settings = newSettings;
        }
    }
}
