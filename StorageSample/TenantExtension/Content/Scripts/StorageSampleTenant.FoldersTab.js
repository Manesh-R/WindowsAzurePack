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
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("viewFolderInfo", "View Info", Exp.UI.CommandIconDescriptor.getWellKnown("viewinfo"), true, null, onViewInfo));
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("deleteFolder", "Delete", Exp.UI.CommandIconDescriptor.getWellKnown("delete"), true, null, onDelete));
        Exp.UI.Commands.update();
    }

    // Command handler : Delete
    function onDelete() {
        var cachedSelectedRow = selectedRow;
        waz.interaction.confirm("Do you want to delete folder {0}?".format(cachedSelectedRow.FolderName))
            .done(function () {
                var promise = controller.deleteFolderAsync(cachedSelectedRow.SubscriptionId, cachedSelectedRow.FolderId);
                waz.interaction.showProgress(
                    promise,
                    {
                        initialText: "Deleting folder {0}".format(cachedSelectedRow.FolderName),
                        successText: "Successfully deleted folder {0}".format(cachedSelectedRow.FolderName),
                        failureText: "Failed to delete folder {0}".format(cachedSelectedRow.FolderName)
                    });
                promise.done(function () {
                    StorageSampleTenantExtension.Controller.getFoldersDataSet(true);
                });
            });
    }

    // Command handler : View Info
    function onViewInfo() {
        var data = {
            name: selectedRow.FolderName,
            // TODO: Get user friendly name of subscription from Exp.Rdfe
            subscription: selectedRow.SubscriptionId,
            url: selectedRow.URL,

            resources: {
                header: 'Folder Details',
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
                        template: "FolderInfoDialog",
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
        var foldersDataSet = controller.getFoldersDataSet(true);

        // TODO: DataSet is coming as null initially. Need to troubleshoot and fix this one.
        //       This problem is not there for SQL provider for example. Need to find out why it comes for StorageSampleTenant.
        if (foldersDataSet == null) {
            setTimeout(function () { loadTab(extension, renderArea, initData); }, 1000);
            return;
        }

        var foldersData = foldersDataSet.data,
            columns = [
                // We need to mark type of column as 'navigation' to enable drill-down.
                { name: "ID", field: "FolderId", sortable: false, type: "navigation", navigationField: "FolderId" },
                { name: "Name", field: "FolderName", sortable: false },
                { name: "Share", field: "ShareId", filterable: false, sortable: false },
                { name: "Subscription Id", field: "SubscriptionId", filterable: false, sortable: false },
                { name: "URL", field: "URL", filterable: false, sortable: false }
            ];

        observableGrid = renderArea.find(".gridFolder")
            .wazObservableGrid("destroy")
            .wazObservableGrid({
                lastSelectedRow: null,
                data: foldersData,
                keyField: "FolderId",
                columns: columns,
                gridOptions: {
                    rowSelect: onRowSelected
                },
                emptyListOptions: {
                    extensionName: "StorageSampleTenantExtension",
                    templateName: "FoldersTabEmpty",
                    arrowLinkSelector: ("{0} .new-folder-link").format(renderArea.selector),
                    arrowLinkAction: openCreateQuickMenu
                }
            });
    }
    
    function openCreateQuickMenu() {
        Exp.Drawer.openMenu("StorageSampleMenuItem/QuickCreateFolder");
    }

    function forceRefreshGridData() {
        try {
            // When we navigate to the tab, sometimes this method is called before observableGrid is not intialized, which will throw exception.
            observableGrid.wazObservableGrid("refreshData");
        } catch (e) {
            // When the grid fails to refresh, we still need to refresh the underlying dataset to make sure it has latest data; otherwise will cause data inconsistent.
            // TODO: When we send request to tenant, we need to provide subscription id as well.
            // Exp.Data.forceRefresh(StorageSampleTenantExtension.Controller.getSharesDataSetInfo().dataSetName);
        }
    }

    function cleanUp() {
        if (observableGrid) {
            observableGrid.wazObservableGrid("destroy");
            observableGrid = null;
        }
    }

    global.StorageSampleTenantExtension = global.StorageSampleTenantExtension || {};
    global.StorageSampleTenantExtension.FoldersTab = {
        loadTab: loadTab,
        cleanUp: cleanUp,
        forceRefreshGridData: forceRefreshGridData,
        statusIcons: statusIcons
    };
})(jQuery, this);
