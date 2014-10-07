/// <reference path="../../scripts/_references.js" />
/*globals Exp,AutomationAdminExtension*/
/// <dictionary>runbook,runbooks,Runbooks,runbookName,runbookDescription,runbookTags</dictionary>

(function ($, global, shell, exp, resources, undefined) {
    "use strict";

    function showQuickCreateShare() {
        //exp.Drawer.openMenu("StorageSampleAdminExtension/QuickCreate");
    }

    function getQuickCreateShareMenuItem() {
        return {
            name: "QuickCreate",
            displayName: "Share",
            description: "Create a new share mapping to a network share",
            template: "shareQuickCreate",
            label: "CREATE",
            data: null,
            opening: function (object) {
            },
            open: function () {
                shell.UI.Validation.setValidationContainer("#shareQuickCreateForm");
            },
            ok: function (object) {
                //if (!shell.UI.Validation.validateContainer("#shareQuickCreateForm")) {
                //    // This will prevent the drawer from closing. The user may then fix the errors or click the "Close" button instead.
                //    return false;
                //}

                return StorageSampleAdminExtension.Actions.createStorageShare(
                            object.fields['shareName'],
                            object.fields['shareTotalSpace'],
                            object.fields['shareNetworkSharePath']
                        )
                    .done(function () { return true; })
                    .fail(function () { return false; });
            }
        };
    }

    shell.Namespace.define("StorageSampleAdminExtension.Menu", {
        getQuickCreateShareMenuItem: getQuickCreateShareMenuItem,
        showQuickCreateShare: showQuickCreateShare
    });

})(jQuery, this, Shell, Exp, StorageSampleAdminExtension.Resources);
