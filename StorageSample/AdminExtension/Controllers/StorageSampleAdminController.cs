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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Azure.Portal.Configuration;
using Microsoft.WindowsAzurePack.Samples.DataContracts;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.AdminExtension.Models;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;
using Microsoft.WindowsAzurePack.Samples;
using Microsoft.WindowsAzurePack.Samples.HelloWorld.Common;


namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.AdminExtension.Controllers
{
    [RequireHttps]
    [OutputCache(Location = OutputCacheLocation.None)]
    [PortalExceptionHandler]
    public sealed class StorageSampleAdminController : ExtensionController
    {
        private static readonly string adminAPIUri = OnPremPortalConfiguration.Instance.RdfeAdminUri;
        //This model is used to show registered resource provider information
        public EndpointModel StorageSampleServiceEndPoint { get; set; }

        /// <summary>
        /// Gets the admin settings.
        /// </summary>
        [HttpPost]
        [ActionName("AdminSettings")]
        public async Task<JsonResult> GetAdminSettings()
        {
            try
            {
                var resourceProvider = await ClientFactory.AdminManagementClient.GetResourceProviderAsync
                                                            (StorageSampleClient.RegisteredServiceName, Guid.Empty.ToString());

                this.StorageSampleServiceEndPoint = EndpointModel.FromResourceProviderEndpoint(resourceProvider.AdminEndpoint);
                return this.JsonDataSet(this.StorageSampleServiceEndPoint);
            }
            catch (ManagementClientException managementException)
            {
                // 404 means the Storage Sample resource provider is not yet configured, return an empty record.
                if (managementException.StatusCode == HttpStatusCode.NotFound)
                {
                    return this.JsonDataSet(new EndpointModel());
                }

                //Just throw if there is any other type of exception is encountered
                throw;
            }
        }

        /// <summary>
        /// Update admin settings => Register Resource Provider
        /// </summary>
        /// <param name="newSettings">The new settings.</param>
        [HttpPost]
        [ActionName("UpdateAdminSettings")]
        public async Task<JsonResult> UpdateAdminSettings(EndpointModel newSettings)
        {
            this.ValidateInput(newSettings);

            ResourceProvider storageSampleResourceProvider;
            string errorMessage = string.Empty;

            try
            {
                //Check if resource provider is already registered or not
                storageSampleResourceProvider = await ClientFactory.AdminManagementClient.GetResourceProviderAsync(StorageSampleClient.RegisteredServiceName, Guid.Empty.ToString());
            }
            catch (ManagementClientException exception)
            {
                // 404 means the Storage Sample resource provider is not yet configured, return an empty record.
                if (exception.StatusCode == HttpStatusCode.NotFound)
                {
                    storageSampleResourceProvider = null;
                }
                else
                {
                    //Just throw if there is any other type of exception is encountered
                    throw;
                }
            }

            if (storageSampleResourceProvider != null)
            {
                //Resource provider already registered so lets update endpoint
                storageSampleResourceProvider.AdminEndpoint = newSettings.ToAdminEndpoint();
                storageSampleResourceProvider.TenantEndpoint = newSettings.ToTenantEndpoint();
                storageSampleResourceProvider.NotificationEndpoint = newSettings.ToNotificationEndpoint();
                storageSampleResourceProvider.UsageEndpoint = newSettings.ToUsageEndpoint();
            }
            else
            {
                //Resource provider not registered yet so lets register new one now
                storageSampleResourceProvider = new ResourceProvider()
                {
                    Name = StorageSampleClient.RegisteredServiceName,
                    DisplayName = "Storage Sample",
                    InstanceDisplayName = StorageSampleClient.RegisteredServiceName + " Instance",
                    Enabled = true,
                    PassThroughEnabled = true,
                    AllowAnonymousAccess = false,
                    AdminEndpoint = newSettings.ToAdminEndpoint(),
                    TenantEndpoint = newSettings.ToTenantEndpoint(),
                    NotificationEndpoint = newSettings.ToNotificationEndpoint(),
                    UsageEndpoint = newSettings.ToUsageEndpoint(),
                    MaxQuotaUpdateBatchSize = 3 // Check link http://technet.microsoft.com/en-us/library/dn520926(v=sc.20).aspx
                };
            }

            var testList = new ResourceProviderVerificationTestList()
                               {
                                   new ResourceProviderVerificationTest()
                                   {
                                       TestUri = new Uri(StorageSampleAdminController.adminAPIUri + StorageSampleClient.AdminSettings),
                                       IsAdmin = true
                                   }
                               };
            try
            {
                // Resource Provider Verification to ensure given endpoint and username/password is correct
                // Only validate the admin RP since we don't have a tenant subscription to do it.
                var result = await ClientFactory.AdminManagementClient.VerifyResourceProviderAsync(storageSampleResourceProvider, testList);
                if (result.HasFailures)
                {
                    throw new HttpException("Invalid endpoint or bad username/password");
                }
            }
            catch (ManagementClientException ex)
            {
                throw new HttpException("Invalid endpoint or bad username/password " + ex.Message.ToString());
            }

            //Finally Create Or Update resource provider
            Task<ResourceProvider> rpTask = (string.IsNullOrEmpty(storageSampleResourceProvider.Name) || String.IsNullOrEmpty(storageSampleResourceProvider.InstanceId))
                                                ? ClientFactory.AdminManagementClient.CreateResourceProviderAsync(storageSampleResourceProvider)
                                                : ClientFactory.AdminManagementClient.UpdateResourceProviderAsync(storageSampleResourceProvider.Name, storageSampleResourceProvider.InstanceId, storageSampleResourceProvider);

            try
            {
                await rpTask;
            }
            catch (ManagementClientException e)
            {
                throw e;
            }

            return this.Json(newSettings);
        }

        /// <summary>
        /// Gets all File Servers.
        /// </summary>
        [HttpPost]
        [ActionName("Locations")]
        public async Task<JsonResult> GetAllLocations()
        {
            try
            {
                var fileServers = await ClientFactory.StorageSampleClient.GetLocationListAsync();
                var fileServerModel = fileServers.Select(d => new LocationModel(d)).ToList();
                return this.JsonDataSet(fileServerModel);
            }
            catch (HttpRequestException)
            {
                // Returns an empty collection if the HTTP request to the API fails
                return this.JsonDataSet(new LocationList());
            }
        }

        /// <summary>
        /// Gets all File Servers.
        /// </summary>
        [HttpPost]
        [ActionName("CreateLocation")]
        public async Task<JsonResult> CreateLocation(Location location)
        {
            try
            {
                await ClientFactory.StorageSampleClient.AddLocationAsync(location);
                return this.JsonDataSet(new object());
            }
            catch (HttpRequestException)
            {
                // Returns an empty collection if the HTTP request to the API fails
                return this.JsonDataSet(new object());
            }
        }

        private void ValidateInput(EndpointModel newSettings)
        {
            if (newSettings == null)
            {
                throw new ArgumentNullException("newSettings");
            }

            if (String.IsNullOrEmpty(newSettings.EndpointAddress))
            {
                throw new ArgumentNullException("EndpointAddress");
            }

            if (String.IsNullOrEmpty(newSettings.Username))
            {
                throw new ArgumentNullException("Username");
            }

            // Note: We do not run validation on password, as password is not null, only when a change is required.
        }
    }
}
