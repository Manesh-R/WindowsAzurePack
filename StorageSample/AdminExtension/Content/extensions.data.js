(function (global, undefined) {
    "use strict";

    var extensions = [{
        name: "StorageSampleAdminExtension",
        displayName: "Storage Sample",
        iconUri: "/Content/StorageSampleAdmin/TestTeam.png",
        iconShowCount: false,
        iconTextOffset: 11,
        iconInvertTextColor: true,
        displayOrderHint: 51
    }];

    global.Shell.Internal.ExtensionProviders.addLocal(extensions);
})(this);