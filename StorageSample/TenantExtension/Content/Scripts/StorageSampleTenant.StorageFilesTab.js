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
        //Exp.UI.Commands.Contextual.add(new Exp.UI.Command("viewContainerInfo", "View Info", Exp.UI.CommandIconDescriptor.getWellKnown("viewinfo"), true, null, onViewInfo));
        //Exp.UI.Commands.Contextual.add(new Exp.UI.Command("deleteContainer", "Delete", Exp.UI.CommandIconDescriptor.getWellKnown("delete"), true, null, onDelete));
        Exp.UI.Commands.update();
    }

    // Public
    function loadTab(extension, renderArea, initData) {
        var columns = [
                { name: "ID", field: "StorageFileId", sortable: false },
                { name: "Name", field: "StorageFileName", sortable: false },
                { name: "Size (KB)", field: "TotalSize", filterable: false, sortable: false },
            ];

        var promise = controller.getStorageFilesAsync(initData.subscriptionId, initData.containerId);
        promise.done(function (response) {
            if (response && response.data) {
                observableGrid = renderArea.find(".gridContainer")
                                    .wazObservableGrid("destroy")
                                    .wazObservableGrid({
                                        lastSelectedRow: null,
                                        data: response.data,
                                        keyField: "StorageFileId",
                                        columns: columns,
                                        gridOptions: {
                                            rowSelect: onRowSelected
                                        },
                                        emptyListOptions: {
                                            extensionName: "StorageSampleTenantExtension",
                                            templateName: "StorageFilesTabEmpty",
                                        }
                                    });
            }
        });
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
    global.StorageSampleTenantExtension.StorageFilesTab = {
        loadTab: loadTab,
        cleanUp: cleanUp,
        forceRefreshGridData: forceRefreshGridData,
        statusIcons: statusIcons
    };
})(jQuery, this);
