/*globals window,jQuery,cdm,StorageSampleTenantExtension,waz,Exp*/
(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/StorageSampleTenant",
        listContainersUrl = baseUrl + "/ListContainers",
        createContainerUrl = baseUrl + "/CreateContainer",
        listLocationsUrl = baseUrl + "/ListLocations",
        domainType = "StorageSample";

    function navigateToListView() {
        Shell.UI.Navigation.navigate("#Workspaces/{0}/storagesample".format(StorageSampleTenantExtension.name));
    }

    function getContainers(subscriptionIds) {
        return makeAjaxCall(listContainersUrl, { subscriptionIds: subscriptionIds }).data;
    }

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data
        });
    }

    function getLocalPlanDataSet() {
        return Exp.Data.getLocalDataSet(planListUrl);
    }

    function getcontainersData(subscriptionId) {
        return Exp.Data.getData("container{0}".format(subscriptionId), {
            ajaxData: {
                subscriptionIds: subscriptionId
            },
            url: listContainersUrl,
            forceCacheRefresh: true
        });
    }

    // TODO: Can we use the waz.dataWrapper in the sample?
    function createContainer(subscriptionId, containerName, size, fileServerName) {
        return new waz.dataWrapper(Exp.Data.getLocalDataSet(listContainersUrl))
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
        listContainersUrl: listContainersUrl,
        listLocationsUrl: listLocationsUrl,
        createContainerUrl: createContainerUrl,
        getContainers: getContainers,
        getLocalPlanDataSet: getLocalPlanDataSet,
        getcontainersData: getcontainersData,
        navigateToListView: navigateToListView
    };
})(jQuery, this);
