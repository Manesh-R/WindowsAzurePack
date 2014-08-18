/*globals window,jQuery,cdm,StorageSampleTenantExtension,waz,Exp*/
(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/StorageSampleTenant",
        containerListUrl = baseUrl + "/ListContainers",
        containerDeleteUrl = baseUrl + "/DeleteContainer",
        createContainerUrl = baseUrl + "/CreateContainer",
        listLocationsUrl = baseUrl + "/ListLocations",
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

    function deleteContainer(subscriptionId, containerId) {
        return Shell.Net.ajaxPost({
                data: {
                    subscriptionId: subscriptionId,
                    containerId: containerId
                },
                url: baseUrl + "/DeleteContainer"
            });
    }

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data
        });
    }

    // TODO: Can we use the waz.dataWrapper in the sample?
    function createContainer(subscriptionId, containerName, size, fileServerName) {
        return new waz.dataWrapper(getContainersDataSet())
            .add(
            {
                Name: containerName,
                SubscriptionId: subscriptionId,
                Size: size,
                FileServerName: fileServerName
            },
            Shell.Net.ajaxPost({
                data: {
                    subscriptionId: subscriptionId,
                    Name: containerName,
                    Size: size,
                    FileServerName: fileServerName
                },
                url: baseUrl + "/CreateContainer"
            })
        );
    }

    global.StorageSampleTenantExtension = global.StorageSampleTenantExtension || {};
    global.StorageSampleTenantExtension.Controller = {
        createContainer: createContainer,
        containerListUrl: containerListUrl,
        listLocationsUrl: listLocationsUrl,
        createContainerUrl: createContainerUrl,
        getContainersDataSet: getContainersDataSet,
        deleteContainer: deleteContainer
    };
})(jQuery, this);
