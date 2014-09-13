/*globals window,jQuery,cdm,StorageSampleTenantExtension,waz,Exp*/
(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/StorageSampleTenant",
        containerListUrl = baseUrl + "/ListContainers",
        constants = global.StorageSampleTenantExtension.Constants;

    function getContainersDataSet(triggerRefreshing) {
        if (triggerRefreshing) {
            Exp.Data.getData({
                url: containerListUrl,
                ajaxData: {
                    subscriptionIds: Exp.Rdfe.getSubscriptionIdsRegisteredToService(constants.serviceName).subscriptionIds
                },
                forceCacheRefresh: true
            });
        }

        return Exp.Data.getLocalDataSet(containerListUrl);
    }

    function createContainerAsync(subscriptionId, containerName, locationId) {
        var data = {};
        data.subscriptionId = subscriptionId;

        data.container = {};
        data.container.ContainerName = containerName;
        data.container.LocationId = locationId;

        return Shell.Net.ajaxPost({
            data: data,
            url: baseUrl + "/CreateContainer"
        });
    }

    function deleteContainerAsync(subscriptionId, containerId) {
        return Shell.Net.ajaxPost({
                data: {
                    subscriptionId: subscriptionId,
                    containerId: containerId
                },
                url: baseUrl + "/DeleteContainer"
            });
    }

    function getLocationsAsync() {
        var subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService(constants.serviceName);
        return Shell.Net.ajaxPost({
            url: baseUrl + "/ListLocations",
            data: {
                subscriptionIds: subscriptionRegisteredToService[0].id
            }
        });
    }

    function getStorageFilesAsync(subscriptionId, containerId) {
        return Shell.Net.ajaxPost({
            data: {
                subscriptionId: subscriptionId,
                containerId: containerId
            },
            url: baseUrl + "/ListStorageFiles"
        });
    }

    function uploadFileAsync(formSelector) {
        var _deferred = $.Deferred();
        global.AppExtension.UI.fileUpload({
            formSelector: formSelector,
            url: baseUrl + "/UploadStorageFile",
            onComplete: function (result) {
                _deferred.resolve(result);
            },
            onError: function (result) {
                _deferred.reject(result);
            }
        });
        return _deferred.promise();
    }

    global.StorageSampleTenantExtension = global.StorageSampleTenantExtension || {};
    global.StorageSampleTenantExtension.Controller = {
        getContainersDataSet: getContainersDataSet,
        createContainerAsync: createContainerAsync,
        deleteContainerAsync: deleteContainerAsync,
        getLocationsAsync: getLocationsAsync,
        getStorageFilesAsync: getStorageFilesAsync,
        uploadFileAsync: uploadFileAsync
    };
})(jQuery, this);
