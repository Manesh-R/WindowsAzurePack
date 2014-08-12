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

using System.Web.Http;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "AdminSettings",
               routeTemplate: "admin/settings",
               defaults: new { controller = "AdminSettings" });

            config.Routes.MapHttpRoute(
                name: "AdminLocations",
                routeTemplate: "admin/locations",
                defaults: new { controller = "Locations" });

            config.Routes.MapHttpRoute(
                name: "TenantContainers",
                routeTemplate: "subscriptions/{subscriptionId}/containers",
                defaults: new { controller = "Containers" });

            config.Routes.MapHttpRoute(
                name: "TenantLocations",
                routeTemplate: "subscriptions/{subscriptionId}/locations",
                defaults: new { controller = "Locations" });

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

            config.Routes.MapHttpRoute(
               name: "Usage",
               routeTemplate: "usage",
               defaults: new { controller = "Usage" });
        }
    }
}
