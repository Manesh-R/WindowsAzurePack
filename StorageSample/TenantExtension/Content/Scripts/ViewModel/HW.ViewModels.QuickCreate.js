/// <disable>JS2076.IdentifierIsMiscased</disable>
(function (global, $, undefined) {
    "use strict";

    // Variables    
    var subscriptions;
    var subscriptionId ;
    var containerName;
    var size;
    var fileServerName;

    function QuickCreateViewModel() {
        this.subscriptions = this.getSubscriptions();
        this.subscriptionId = this.subscriptions.length ? this.subscriptions[0].id : null;
        _selectors = {
            container: "#hw-create-container-container",
            containerName: "#containerName",
            size: "#size",
            subscriptions: "#subscriptions",
            fileServerName: "#fileServerName"
        }
    }
    
    function onOpened() {
        // using AppExtension subscription drop down as it handles disabled and single subscriptions properly
        global.AppExtension.UserContext.populateOrHideSubscriptionsDropDown("subscriptions", null, null, null, null, "storagesample");
    }

    // Public Methods
    function onOkClicked() {
        global.StorageSampleTenantExtension.Controller.createContainer(this.subscriptionId, this.containerName, this.size, this.fileServerName);
    }

    function getSubscriptions() {
        return global.Exp.Rdfe.getSubscriptionsRegisteredToService(global.StorageSampleTenantExtension.serviceName);
    }

    global.StorageSampleTenantExtension = global.StorageSampleTenantExtension || {};
    global.StorageSampleTenantExtension.ViewModels = {
        QuickCreateViewModel: QuickCreateViewModel,
        onOpened: onOpened,
        onOkClicked: onOkClicked
    };
})(jQuery, this);