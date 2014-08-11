//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.Portal.Configuration;
using Microsoft.WindowsAzurePack.Samples;
using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.AdminExtension
{
    public static class ClientFactory
    {
        //Get Service Management API endpoint
        private static Uri adminApiUri;

        private static BearerMessageProcessingHandler messageHandler;

        //This client is used to communicate with the Storage Sample resource provider
        private static Lazy<StorageSampleClient> storageSampleRestClient = new Lazy<StorageSampleClient>(
           () => new StorageSampleClient(adminApiUri, messageHandler),
           LazyThreadSafetyMode.ExecutionAndPublication);

        //This client is used to communicate with the Admin API
        private static Lazy<AdminManagementClient> adminApiRestClient = new Lazy<AdminManagementClient>(
            () => new AdminManagementClient(adminApiUri, messageHandler),
            LazyThreadSafetyMode.ExecutionAndPublication);

        static ClientFactory()
        {
            adminApiUri = new Uri(OnPremPortalConfiguration.Instance.RdfeAdminUri);
            messageHandler = new BearerMessageProcessingHandler(new WebRequestHandler());
        }

        public static StorageSampleClient StorageSampleClient
        {
            get
            {
                return storageSampleRestClient.Value;
            }
        }

        public static AdminManagementClient AdminManagementClient
        {
            get
            {
                return adminApiRestClient.Value;
            }
        }
    }
}
