/// <reference path="../../scripts/_references.js" />
/*globals Exp,fx,waz,StorageSampleAdminExtension*/
/// <dictionary>runbook,runbooks,Runbook,Runbooks</dictionary>

(function ($, global, shell, exp, resources, constants, undefined) {
    "use strict";

    var createAssetImpl,
        //validation = StorageSampleAdminExtension.Validation,
        controller = StorageSampleAdminExtension.Controller;

    function createStorageLocation(name, totalSpace, networkSharePath) {
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
        data.LocationName = name;
        data.TotalSpace = totalSpace;
        data.NetworkSharePath = networkSharePath;
        var promise = StorageSampleAdminExtension.Controller.makeAjaxCall(StorageSampleAdminExtension.Controller.adminLocationCreateUrl, data);

        global.waz.interaction.showProgress(
            promise,
            {
                initialText: "Creating storage location '" + name + "'.",
                successText: "Successfully created storage location '" + name + "'.",
                failureText: "Failed to create storage location '" + name + "'."
            }
        );

        promise.done(function () {
        });
        promise.always(function () {
            StorageSampleAdminExtension.LocationsTab.forceRefreshLocationsData();
        });

        return promise.promise();
    }

    shell.Namespace.define("StorageSampleAdminExtension.Actions", {
        createStorageLocation: createStorageLocation
    });
})(jQuery, this, Shell, Exp, StorageSampleAdminExtension.Resources, StorageSampleAdminExtension.Constants);
