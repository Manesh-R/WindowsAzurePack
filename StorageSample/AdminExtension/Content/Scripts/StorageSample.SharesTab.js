/*globals window,jQuery,Exp,waz*/
(function ($, global, Shell, Exp, undefined) {
    "use strict";

    var observableGrid,
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
    }

    function loadTab(extension, renderArea, initData) {
        var columns = [
                { name: "Id", field: "ShareId", sortable: false },
                { name: "Name", field: "ShareName", sortable: false },
                { name: "Total Space", field: "TotalSpace", filterable: false, sortable: false },
                { name: "Free Space", field: "FreeSpace", filterable: false, sortable: false },
                { name: "Network File Share", field: "NetworkSharePath", filterable: false, sortable: false },
            ];

        observableGrid = renderArea.find(".grid-container")
            .wazObservableGrid("destroy")
            .wazObservableGrid({
                lastSelectedRow: null,
                data: global.StorageSampleAdminExtension.Controller.getSharesDataSetInfo(),
                keyField: "ShareId",
                columns: columns,
                gridOptions: {
                    rowSelect: onRowSelected
                },
                emptyListOptions: {
                    extensionName: "StorageSampleAdminExtension",
                    templateName: "sharesTabEmpty"
                }
            });
    }

    function cleanUp() {
        if (observableGrid) {
            observableGrid.wazObservableGrid("destroy");
            observableGrid = null;
        }
    }

    function forceRefreshSharesData() {
        try {
            // When we navigate to the tab, sometimes this method is called before observableGrid is not intialized, which will throw exception.
            observableGrid.wazObservableGrid("refreshData");
        } catch (e) {
            // When the grid fails to refresh, we still need to refresh the underlying dataset to make sure it has latest data; otherwise will cause data inconsistent.
            Exp.Data.forceRefresh(StorageSampleAdminExtension.Controller.getSharesDataSetInfo().dataSetName);
        }
    }

    global.StorageSampleAdminExtension = global.StorageSampleAdminExtension || {};
    global.StorageSampleAdminExtension.SharesTab = {
        loadTab: loadTab,
        cleanUp: cleanUp,
        forceRefreshSharesData: forceRefreshSharesData
    };
})(jQuery, this, this.Shell, this.Exp);