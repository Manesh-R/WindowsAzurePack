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

using System.Web.Http;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // All Admin methods that are called via AdminSite are coming to 'admin' route.
            // This is because when resource provider is registered, we used following.
            //  'AdminForwardingAddress' = "http://$hostName/admin";
            config.Routes.MapHttpRoute(
               name: "AdminSettings",
               routeTemplate: "admin/settings",
               defaults: new { controller = "AdminSettings" });

            config.Routes.MapHttpRoute(
                name: "AdminShares",
                routeTemplate: "admin/shares",
                defaults: new { controller = "Shares" });

            config.Routes.MapHttpRoute(
               name: "StorageSampleQuota",
               routeTemplate: "admin/quota",
               defaults: new { controller = "Quota" });

            config.Routes.MapHttpRoute(
               name: "StorageSampleDefaultQuota",
               routeTemplate: "admin/defaultquota",
               defaults: new { controller = "Quota" });

            config.Routes.MapHttpRoute(
               name: "Subscription",
               routeTemplate: "admin/subscriptions",
               defaults: new { controller = "Subscriptions" });

            // All tenant methods will have incoming route as subscriptions/{subscriptionId}
            // This is based on the registeration done for resource provider.
            //     'TenantSourceUriTemplate' = '{subid}/services/storagesample/{*path}';
            //     'TenantTargetUriTemplate' = 'subscriptions/{subid}/{*path}';
            // This means that tenant portal will send request to source uri template, and tenant api site will 
            // forward it to StorageSample API after transforming uri to target uri template.
            config.Routes.MapHttpRoute(
                name: "TenantFolders",
                routeTemplate: "subscriptions/{subscriptionId}/folders",
                defaults: new { controller = "Folders" });
            config.Routes.MapHttpRoute(
                name: "TenantStorageFiles",
                routeTemplate: "subscriptions/{subscriptionId}/folders/{folderId}/files",
                defaults: new { controller = "StorageFiles" });

            config.Routes.MapHttpRoute(
                name: "TenantShares",
                routeTemplate: "subscriptions/{subscriptionId}/shares",
                defaults: new { controller = "Shares" });

            config.Routes.MapHttpRoute(
               name: "Usage",
               routeTemplate: "usage",
               defaults: new { controller = "Usage" });
        }
    }
}
