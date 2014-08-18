/// <reference path="StorageSampletenant.createwizard.js" />
/// <reference path="StorageSampletenant.controller.js" />
/*globals window,jQuery,StorageSampleTenantExtension,Exp,waz,cdm*/
(function ($, global, undefined) {
    "use strict";

    var controller = global.StorageSampleTenantExtension.Controller,
        constants  = global.StorageSampleTenantExtension.Constants,
        observableGrid,
        selectedRow,
        statusIcons = {
            Registered: {
                text: "Registered",
                iconName: "complete"
            },
            Default: {
                iconName: "spinner"
            }
        };

    function onRowSelected(row) {
        if (row) {
            selectedRow = row;
            updateContextualCommands(row);
        }
    }

    function updateContextualCommands(domain) {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("viewContainerInfo", "View Info", Exp.UI.CommandIconDescriptor.getWellKnown("viewinfo"), true, null, onViewInfo));
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("deleteContainer", "Delete", Exp.UI.CommandIconDescriptor.getWellKnown("delete"), true, null, onDelete));
        Exp.UI.Commands.update();
    }

    // Command handler : Delete
    function onDelete() {
        var cachedSelectedRow = selectedRow;
        waz.interaction.confirm("Do you want to delete container {0}?".format(cachedSelectedRow.ContainerName))
            .done(function () {
                var promise = controller.deleteContainerAsync(cachedSelectedRow.SubscriptionId, cachedSelectedRow.ContainerId);
                waz.interaction.showProgress(
                    promise,
                    {
                        initialText: "Deleting container {0}".format(cachedSelectedRow.ContainerName),
                        successText: "Successfully deleted container {0}".format(cachedSelectedRow.ContainerName),
                        failureText: "Failed to delete container {0}".format(cachedSelectedRow.ContainerName)
                    });
                promise.done(function () {
                    StorageSampleTenantExtension.Controller.getContainersDataSet(true);
                });
            });
    }

    // Command handler : View Info
    function onViewInfo() {
        var data = {
            name: selectedRow.ContainerName,
            // TODO: Get user friendly name of subscription from Exp.Rdfe
            subscription: selectedRow.SubscriptionId,
            url: selectedRow.URL,

            resources: {
                header: 'Container Details',
                subHeader: 'Properties',
                nameLabel: 'Name',
                subscriptionLabel: 'Subscription',
                urlLabel: 'Fully Qualified URL',
            }
        };

        cdm.stepWizard({
            extension: "StorageSampleTenantExtension",
            steps: [
                    {
                        template: "ContainerInfoDialog",
                        data: data
                    }
            ]
        });
    }

    // Public
    function loadTab(extension, renderArea, initData) {
        // We need to fetch data for multiple subscription in a single Go.
        // The way SQL Tenant fetches data is by providing the original 'SubscriptionPool'
        // However, HelloWorld tenant is written which accepts a string array.
        // So tweaking in here to get only the subscription ids as an array and send it in POST.

        var containersData = controller.getContainersDataSet(true).data,
            columns = [
                { name: "ID", field: "ContainerId", sortable: false },
                { name: "Name", field: "ContainerName", sortable: false },
                { name: "Location", field: "LocationId", filterable: false, sortable: false },
                { name: "Subscription Id", field: "SubscriptionId", filterable: false, sortable: false },
                { name: "URL", field: "URL", filterable: false, sortable: false }
            ];

        observableGrid = renderArea.find(".gridContainer")
            .wazObservableGrid("destroy")
            .wazObservableGrid({
                lastSelectedRow: null,
                data: containersData,
                keyField: "ContainerId",
                columns: columns,
                gridOptions: {
                    rowSelect: onRowSelected
                },
                emptyListOptions: {
                    extensionName: "StorageSampleTenantExtension",
                    templateName: "ContainersTabEmpty",
                    arrowLinkSelector: ("{0} .new-container-link").format(renderArea.selector),
                    arrowLinkAction: openCreateQuickMenu
                }
            });
    }
    
    function openCreateQuickMenu() {
        Exp.Drawer.openMenu("StorageSampleMenuItem/QuickCreateContainer");
    }

    function forceRefreshGridData() {
        try {
            // When we navigate to the tab, sometimes this method is called before observableGrid is not intialized, which will throw exception.
            observableGrid.wazObservableGrid("refreshData");
        } catch (e) {
            // When the grid fails to refresh, we still need to refresh the underlying dataset to make sure it has latest data; otherwise will cause data inconsistent.
            // TODO: When we send request to tenant, we need to provide subscription id as well.
            // Exp.Data.forceRefresh(StorageSampleTenantExtension.Controller.getLocationsDataSetInfo().dataSetName);
        }
    }

    function cleanUp() {
        if (observableGrid) {
            observableGrid.wazObservableGrid("destroy");
            observableGrid = null;
        }
    }

    global.StorageSampleTenantExtension = global.StorageSampleTenantExtension || {};
    global.StorageSampleTenantExtension.ContainersTab = {
        loadTab: loadTab,
        cleanUp: cleanUp,
        forceRefreshGridData: forceRefreshGridData,
        statusIcons: statusIcons
    };
})(jQuery, this);
