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
        public const string AdminShares = RegisteredPath + "/shares";

        public const string TenantFolders = "{0}/" + RegisteredPath + "/folders";
        public const string TenantShares = "{0}/" + RegisteredPath + "/shares";
        public const string TenantFilesInFolder = "{0}/" + RegisteredPath + "/folders/{1}/files";

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
        /// GetShareList return list of file servers hosted in Storage Sample Resource Provider
        /// </summary>
        /// <returns></returns>
        public async Task<List<Share>> GetShareListAsync()
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, StorageSampleClient.AdminShares));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Share>>();
        }

        /// <summary>
        /// UpdateShare updates existing file server information in Storage Sample Resource Provider
        /// </summary>        
        public async Task UpdateShareAsync(Share share)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminShares);
            var response = await this.httpClient.PutAsJsonAsync<Share>(requestUrl.ToString(), share);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// AddShare adds new file server in Storage Sample Resource Provider
        /// </summary>        
        public async Task AddShareAsync(Share share)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminShares);
            var response = await this.httpClient.PutAsJsonAsync<Share>(requestUrl.ToString(), share);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// DeleteLocatio removes file server in Storage Sample Resource Provider
        /// </summary>        
        public async Task DeleteShareAsync(Share share)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.AdminShares);
            var response = await this.httpClient.DeleteAsync(requestUrl.ToString() + "/" + share.ShareId);
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region Tenant APIs

        /// <summary>
        /// Get list of files with in a specific folder.
        /// </summary>
        /// <returns></returns>
        public async Task<List<StorageFile>> GetFileListForTenantAsync(string subscriptionId, int folderId)
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, StorageSampleClient.TenantFilesInFolder, subscriptionId, folderId));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<StorageFile>>();
        }

        /// <summary>
        /// Upload file to storage folder.
        /// </summary>
        public async Task UploadForTenantAsync(string subscriptionId, int folderId, string fileName, byte[] fileContent)
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, StorageSampleClient.TenantFilesInFolder, subscriptionId, folderId));
            StorageFile file = new StorageFile() { StorageFileName = fileName, FileContent = fileContent };
            await this.PostAsync<StorageFile>(requestUrl, file);
        }

        /// <summary>
        /// GetShareList return list of file servers hosted in Storage Sample Resource Provider
        /// </summary>
        /// <returns></returns>
        public async Task<List<Share>> GetShareListForTenantAsync(string subscriptionId = null)
        {
            var requestUrl = this.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, StorageSampleClient.TenantShares, subscriptionId));

            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Share>>();
        }

        /// <summary>
        /// ListFolders supposed to return list of folders per subscription stored in Storage Sample Resource Provider 
        /// Per subscription shares not implemented for this sample so its returning static common folders for all subscriptions
        /// </summary> 
        public async Task<List<Folder>> ListFoldersAsync(string subscriptionId = null)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.CreateTenantFoldersUri(subscriptionId));
            return await this.GetAsync<List<Folder>>(requestUrl);            
        }
        
        /// <summary>
        /// CreateFolder allows to create new folder for given subscription 
        /// </summary>        
        public async Task CreateFolderAsync(string subscriptionId, Folder folderNameToCreate)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.CreateTenantFoldersUri(subscriptionId));
            await this.PostAsync<Folder>(requestUrl, folderNameToCreate);            
        }

        /// <summary>
        /// UpdateFolder allows to update existing folder for given subscription 
        /// </summary>        
        public async Task UpdateFolderAsync(string subscriptionId, Folder folder)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.CreateTenantFoldersUri(subscriptionId));
            await this.PutAsync<Folder>(requestUrl, folder);            
        }

        /// <summary>
        /// CreateFolder allows to create new folder for given subscription 
        /// </summary>        
        public async Task DeleteFolderAsync(string subscriptionId, int folderId)
        {
            var requestUrl = this.CreateRequestUri(StorageSampleClient.CreateTenantFoldersUri(subscriptionId));
            await this.httpClient.DeleteAsync(requestUrl.ToString() + "?folderId=" + folderId);
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
        /// Common method for making Delete calls
        /// </summary>        
        private async Task DeleteAsync(Uri requestUrl)
        {
            var response = await this.httpClient.DeleteAsync(requestUrl);
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

        private static string CreateTenantFoldersUri(string subscriptionId)
        {
            return string.Format(CultureInfo.InvariantCulture, StorageSampleClient.TenantFolders, subscriptionId);
        }
        #endregion
    }
}
