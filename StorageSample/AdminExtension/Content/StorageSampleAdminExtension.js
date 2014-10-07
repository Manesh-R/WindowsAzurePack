/*globals window,jQuery,Shell,Exp,waz*/

(function (global, $, shell, exp, resources, constants, undefined) {
    "use strict";

    var storageSampleExtensionActivationInit,
        navigation;   

    function clearCommandBar() {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Global.clear();
        Exp.UI.Commands.update();
    }

    function onApplicationStart() {
        Exp.UserSettings.getGlobalUserSetting("Admin-skipQuickStart").then(function (results) {
            var setting = results ? results[0] : null;
            if (setting && setting.Value) {
                global.StorageSampleAdminExtension.settings.skipQuickStart = JSON.parse(setting.Value);
            }
        });
                
        global.StorageSampleAdminExtension.settings.skipQuickStart = false;
    }

    function loadQuickStart(extension, renderArea, renderData) {
        clearCommandBar();
        global.StorageSampleAdminExtension.QuickStartTab.loadTab(renderData, renderArea);
    }

    function loadFileServersTab(extension, renderArea, renderData) {
        global.StorageSampleAdminExtension.FileServersTab.loadTab(renderData, renderArea);
    }

    function loadProductsTab(extension, renderArea, renderData) {
        global.StorageSampleAdminExtension.ProductsTab.loadTab(renderData, renderArea);
    }

    function loadSettingsTab(extension, renderArea, renderData) {
        global.StorageSampleAdminExtension.SettingsTab.loadTab(renderData, renderArea);
    }

    function loadControlsTab(extension, renderArea, renderData) {
        global.StorageSampleAdminExtension.ControlsTab.loadTab(renderData, renderArea);
    }

    function loadSharesTab(extension, renderArea, renderData) {
        global.StorageSampleAdminExtension.SharesTab.loadTab(renderData, renderArea);
    }

    global.storageSampleExtension = global.StorageSampleAdminExtension || {};

    navigation = {
        tabs: [
                {
                    id: "quickStart",
                    displayName: "quickStart",
                    template: "quickStartTab",
                    activated: loadQuickStart
                },
                {
                    id: "shares",
                    displayName: "shares",
                    template: "sharesTab",
                    activated: loadSharesTab
                },
                {
                    id: "settings",
                    displayName: "settings",
                    template: "settingsTab",
                    activated: loadSettingsTab
                }
        ],
        types: [
        ]
    };

    storageSampleExtensionActivationInit = function () {
        var storageSampleExtension = $.extend(this, global.StorageSampleAdminExtension);

        $.extend(storageSampleExtension, {
            displayName: "Storage Sample",
            viewModelUris: [
                global.StorageSampleAdminExtension.Controller.adminSettingsUrl,
                global.StorageSampleAdminExtension.Controller.adminProductsUrl,
            ],
            menuItems: [
                {
                    name: constants.extensionName,
                    displayName: resources.MenuFirstStoryDisplayName,
                    url: "#Workspaces/StorageSampleAdminExtension",
                    description: resources.MenuRunbookQuickCreateDescription,
                    isEnabled: function () {
                        //var isExtensionReady = automationAdminExtension.settings.isAutomationEndpointRegistered && global.Shell.extensionIndex.AutomationAdminExtension.dataIsLoaded;
                        //return {
                        //    enabled: isExtensionReady,
                        //    description: isExtensionReady ? resources.MenuRunbookQuickCreateDescription : resources.AutomationResourceNotAvailable
                        //};
                        return {
                            enabled: true,
                            description: "Create data storage services."
                        }
                    },
                    subMenu: [
                        StorageSampleAdminExtension.Menu.getQuickCreateShareMenuItem()
                    ]
                }
            ],
            settings: {
                skipQuickStart: true
            },
            getResources: function () {
                return resources;
            }
        });

        storageSampleExtension.onApplicationStart = onApplicationStart;        
        storageSampleExtension.setCommands = clearCommandBar();

        Shell.UI.Pivots.registerExtension(storageSampleExtension, function () {
            Exp.Navigation.initializePivots(this, navigation);
        });

        // Finally activate storageSampleExtension 
        $.extend(global.StorageSampleAdminExtension, Shell.Extensions.activate(storageSampleExtension));
    };

    Shell.Namespace.define("StorageSampleAdminExtension", {
        init: storageSampleExtensionActivationInit
    });

})(this, jQuery, Shell, Exp, StorageSampleAdminExtension.Resources, StorageSampleAdminExtension.Constants);
