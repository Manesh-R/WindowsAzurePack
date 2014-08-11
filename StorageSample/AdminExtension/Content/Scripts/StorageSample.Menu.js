/// <reference path="../../scripts/_references.js" />
/*globals Exp,AutomationAdminExtension*/
/// <dictionary>runbook,runbooks,Runbooks,runbookName,runbookDescription,runbookTags</dictionary>

(function ($, global, shell, exp, resources, undefined) {
    "use strict";

    function showQuickCreateLocation() {
        //exp.Drawer.openMenu("StorageSampleAdminExtension/QuickCreate");
    }

    function getQuickCreateLocationMenuItem() {
        return {
            name: "QuickCreate",
            displayName: "Blob Location",
            description: "Create a new blob storage location",
            template: "locationQuickCreate",
            label: "CREATE",
            data: null,
            opening: function (object) {
            },
            open: function () {
                shell.UI.Validation.setValidationContainer("#locationQuickCreateForm");
            },
            ok: function (object) {
                //if (!shell.UI.Validation.validateContainer("#locationQuickCreateForm")) {
                //    // This will prevent the drawer from closing. The user may then fix the errors or click the "Close" button instead.
                //    return false;
                //}

                return StorageSampleAdminExtension.Actions.createStorageLocation(
                            object.fields['locationName'],
                            object.fields['locationTotalSpace'],
                            object.fields['locationNetworkSharePath']
                        )
                    .done(function () { return true; })
                    .fail(function () { return false; });
            }
        };
    }

    shell.Namespace.define("StorageSampleAdminExtension.Menu", {
        getQuickCreateLocationMenuItem: getQuickCreateLocationMenuItem,
        showQuickCreateLocation: showQuickCreateLocation
    });

})(jQuery, this, Shell, Exp, StorageSampleAdminExtension.Resources);
