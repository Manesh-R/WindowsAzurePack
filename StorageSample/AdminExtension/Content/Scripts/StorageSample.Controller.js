/*globals window,jQuery,cdm, StorageSampleAdminExtension*/
(function ($, global, shell, exp, undefined) {
    "use strict";

    var baseUrl = "/StorageSampleAdmin",
        adminSettingsUrl = baseUrl + "/AdminSettings",
        adminProductsUrl = baseUrl + "/Products",
        adminFileServersUrl = baseUrl + "/FileServers",
        adminSharesUrl = baseUrl + "/Shares",
        adminShareCreateUrl = baseUrl + "/CreateShare";

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

    function getSharesDataSetInfo() {
        return {
            url: adminSharesUrl,
            dataSetName: adminSharesUrl
        };
    }

    // Public
    global.StorageSampleAdminExtension = global.StorageSampleAdminExtension || {};
    global.StorageSampleAdminExtension.Controller = {
        adminSettingsUrl: adminSettingsUrl,
        adminProductsUrl: adminProductsUrl,
        adminFileServersUrl: adminFileServersUrl,
        adminSharesUrl: adminSharesUrl,
        adminShareCreateUrl: adminShareCreateUrl,
        updateAdminSettings: updateAdminSettings,
        getCurrentAdminSettings: getCurrentAdminSettings,
        invalidateAdminSettingsCache: invalidateAdminSettingsCache,
        isResourceProviderRegistered: isResourceProviderRegistered,
        getSharesDataSetInfo: getSharesDataSetInfo,
        makeAjaxCall: makeAjaxCall
    };
})(jQuery, this, Shell, Exp);
