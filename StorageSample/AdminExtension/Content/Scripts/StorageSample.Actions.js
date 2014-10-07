/// <reference path="../../scripts/_references.js" />
/*globals Exp,fx,waz,StorageSampleAdminExtension*/
/// <dictionary>runbook,runbooks,Runbook,Runbooks</dictionary>

(function ($, global, shell, exp, resources, constants, undefined) {
    "use strict";

    var createAssetImpl,
        //validation = StorageSampleAdminExtension.Validation,
        controller = StorageSampleAdminExtension.Controller;

    function createStorageShare(name, totalSpace, networkSharePath) {
        /// <summary>
        /// Creates the runbook using the provided parameter values.
        /// </summary>
        /// <param name="name" type="Object" maybeNull="false" optional="false">
        /// Runbook name.
        /// </param>
        /// <param name="description" type="Object" maybeNull="false" optional="false">
        /// Runbook description.
        /// </param>
        /// <param name="tags" type="Object" maybeNull="false" optional="false">
        /// Tags associated with the runbook.
        /// </param>

        //validation.throwIfStringNullOrEmpty(name, "name");

        var data = { };
        data.ShareName = name;
        data.TotalSpace = totalSpace;
        data.NetworkSharePath = networkSharePath;
        var promise = StorageSampleAdminExtension.Controller.makeAjaxCall(StorageSampleAdminExtension.Controller.adminShareCreateUrl, data);

        global.waz.interaction.showProgress(
            promise,
            {
                initialText: "Creating storage share '" + name + "'.",
                successText: "Successfully created storage share '" + name + "'.",
                failureText: "Failed to create storage share '" + name + "'."
            }
        );

        promise.done(function () {
        });
        promise.always(function () {
            StorageSampleAdminExtension.SharesTab.forceRefreshSharesData();
        });

        return promise.promise();
    }

    shell.Namespace.define("StorageSampleAdminExtension.Actions", {
        createStorageShare: createStorageShare
    });
})(jQuery, this, Shell, Exp, StorageSampleAdminExtension.Resources, StorageSampleAdminExtension.Constants);
