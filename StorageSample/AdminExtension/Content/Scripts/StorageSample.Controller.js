/*globals window,jQuery,cdm, StorageSampleAdminExtension*/
(function ($, global, shell, exp, undefined) {
    "use strict";

    var baseUrl = "/StorageSampleAdmin",
        adminSettingsUrl = baseUrl + "/AdminSettings",
        adminProductsUrl = baseUrl + "/Products",
        adminFileServersUrl = baseUrl + "/FileServers",
        adminLocationsUrl = baseUrl + "/Locations",
        adminLocationCreateUrl = baseUrl + "/CreateLocation";

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data
        });
    }

    function updateAdminSettings(newSettings) {
        return makeAjaxCall(baseUrl + "/UpdateAdminSettings", newSettings);
    }

    function invalidateAdminSettingsCache() {
        return global.Exp.Data.getData({
            url: global.StorageSampleAdminExtension.Controller.adminSettingsUrl,
            dataSetName: StorageSampleAdminExtension.Controller.adminSettingsUrl,
            forceCacheRefresh: true
        });
    }

    function getCurrentAdminSettings() {
        return makeAjaxCall(global.StorageSampleAdminExtension.Controller.adminSettingsUrl);
    }

    function isResourceProviderRegistered() {
        global.Shell.UI.Spinner.show();
        global.StorageSampleAdminExtension.Controller.getCurrentAdminSettings()
        .done(function (response) {
            if (response && response.data.EndpointAddress) {
                return true;
            }
            else {
                return false;
            }
        })
         .always(function () {
             global.Shell.UI.Spinner.hide();
         });
    }

    function getLocationsDataSetInfo() {
        return {
            url: adminLocationsUrl,
            dataSetName: adminLocationsUrl
        };
    }

    // Public
    global.StorageSampleAdminExtension = global.StorageSampleAdminExtension || {};
    global.StorageSampleAdminExtension.Controller = {
        adminSettingsUrl: adminSettingsUrl,
        adminProductsUrl: adminProductsUrl,
        adminFileServersUrl: adminFileServersUrl,
        adminLocationsUrl: adminLocationsUrl,
        adminLocationCreateUrl: adminLocationCreateUrl,
        updateAdminSettings: updateAdminSettings,
        getCurrentAdminSettings: getCurrentAdminSettings,
        invalidateAdminSettingsCache: invalidateAdminSettingsCache,
        isResourceProviderRegistered: isResourceProviderRegistered,
        getLocationsDataSetInfo: getLocationsDataSetInfo,
        makeAjaxCall: makeAjaxCall
    };
})(jQuery, this, Shell, Exp);
