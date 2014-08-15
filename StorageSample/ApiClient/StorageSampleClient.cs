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
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient
{
    /// <summary>
    /// This is client of Storage Sample Resource Provider 
    /// This client is used by admin and tenant extensions to make call to Storage Sample Resource Provider
    /// In real world you should have seperate clients of admin and tenant extensions    
    /// </summary>
    public class StorageSampleClient
    {        
        public const string RegisteredServiceName = "storagesample";
        public const string RegisteredPath = "services/" + RegisteredServiceName;
        public const string AdminSettings = RegisteredPath + "/settings";
        public const string AdminLocations = RegisteredPath + "/locations";

        public const string TenantContainers = "{0}/" + RegisteredPath + "/containers";
        public const string TenantLocations = "{0}/" + RegisteredPath + "/locations";

        public Uri BaseEndpoint { get; set; }
        public HttpClient httpClient;

        /// <summary>
        /// This constructor takes BearerMessageProcessingHandler which reads token as attach to each request
        /// </summary>
        /// <param name="baseEndpoint"></param>
        /// <param name="handler"></param>
        public StorageSampleClient(Uri baseEndpoint, MessageProcessingHandler handler)
        {
            if (baseEndpoint == null) 
            {
                throw new ArgumentNullException("baseEndpoint"); 
            }

            this.BaseEndpoint = baseEndpoint;

            this.httpClient = new HttpClient(handler);
        }

        public StorageSampleClient(Uri baseEndpoint, string bearerToken, TimeSpan? timeout = null)
        {
            if (baseEndpoint == null) 
            { 
                throw new ArgumentNullException("baseEndpoint"); 
            }

            this.BaseEndpoint = baseEndpoint;

            this.httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            if (timeout.HasValue)
            {
                this.httpClient.Timeout = timeout.Value;
            }
        }
       
        #region Admin APIs
        /// <summary>
        /// GetAdminSettings returns Storage Sample Resource Provider endpoint information if its registered with Admin API
        /// </summary>
        /// <returns></returns>
        public async Task<AdminSettings> GetAdminSettingsAsync()
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminSettings);

            // For simplicity, we make a request synchronously.
            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<AdminSettings>();
        }

        /// <summary>
        /// UpdateAdminSettings registers Storage Sample Resource Provider endpoint information with Admin API
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAdminSettingsAsync(AdminSettings newSettings)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminSettings);
            var response = await this.httpClient.PutAsJsonAsync<AdminSettings>(requestUrl.ToString(), newSettings);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// GetLocationList return list of file servers hosted in Storage Sample Resource Provider
        /// </summary>
        /// <returns></returns>
        public async Task<List<Location>> GetLocationListAsync()
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, StorageSampleClient.AdminLocations));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Location>>();
        }

        /// <summary>
        /// UpdateLocation updates existing file server information in Storage Sample Resource Provider
        /// </summary>        
        public async Task UpdateLocationAsync(Location location)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminLocations);
            var response = await this.httpClient.PutAsJsonAsync<Location>(requestUrl.ToString(), location);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// AddLocation adds new file server in Storage Sample Resource Provider
        /// </summary>        
        public async Task AddLocationAsync(Location location)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminLocations);
            var response = await this.httpClient.PutAsJsonAsync<Location>(requestUrl.ToString(), location);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// DeleteLocatio removes file server in Storage Sample Resource Provider
        /// </summary>        
        public async Task DeleteLocationAsync(Location location)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminLocations);
            var response = await this.httpClient.DeleteAsync(requestUrl.ToString() + "/" + location.LocationId);
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region Tenant APIs

        /// <summary>
        /// GetLocationList return list of file servers hosted in Storage Sample Resource Provider
        /// </summary>
        /// <returns></returns>
        public async Task<List<Location>> GetLocationListForTenantAsync(string subscriptionId = null)
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, StorageSampleClient.TenantLocations, subscriptionId));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Location>>();
        }

        /// <summary>
        /// ListContainers supposed to return list of containers per subscription stored in Storage Sample Resource Provider 
        /// Per subscription shares not implemented for this sample so its returning static common containers for all subscriptions
        /// </summary> 
        public async Task<List<Container>> ListContainersAsync(string subscriptionId = null)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.CreateTenantContainersUri(subscriptionId));
            return await this.GetAsync<List<Container>>(requestUrl);            
        }
        
        /// <summary>
        /// CreateContainer allows to create new container for given subscription 
        /// </summary>        
        public async Task CreateContainerAsync(string subscriptionId, Container containerNameToCreate)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.CreateTenantContainersUri(subscriptionId));
            await this.PostAsync<Container>(requestUrl, containerNameToCreate);            
        }        

        /// <summary>
        /// UpdateContainer allows to update existing container for given subscription 
        /// </summary>        
        public async Task UpdateContainerAsync(string subscriptionId, Container containerNameToUpdate)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.CreateTenantContainersUri(subscriptionId));
            await this.PutAsync<Container>(requestUrl, containerNameToUpdate);            
        }        
        #endregion

        #region Private Methods
        /// <summary>
        /// Common method for making GET calls
        /// </summary>        
        private async Task<T> GetAsync<T>(Uri requestUrl)
        {         
            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>        
        private async Task PostAsync<T>(Uri requestUrl, T content)
        {            
            var response = await this.httpClient.PostAsXmlAsync<T>(requestUrl.ToString(), content);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Common method for making PUT calls
        /// </summary>        
        private async Task PutAsync<T>(Uri requestUrl, T content)
        {            
            var response = await this.httpClient.PutAsJsonAsync<T>(requestUrl.ToString(), content);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Common method for making Request Uri's
        /// </summary>        
        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(this.BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        private static string CreateTenantContainersUri(string subscriptionId)
        {
            return string.Format(CultureInfo.InvariantCulture, StorageSampleClient.TenantContainers, subscriptionId);
        }
        #endregion
    }
}
