/// <reference path="scripts/storageSampleTenant.createwizard.js" />
/// <reference path="scripts/storageSampleTenant.controller.js" />
/*globals window,jQuery,Shell, StorageSampleTenantExtension, Exp*/

(function ($, global, undefined) {
    "use strict";

    var resources = [],
        controller = global.StorageSampleTenantExtension.Controller,
        constants = global.StorageSampleTenantExtension.Constants,
        StorageSampleTenantExtensionActivationInit,
        navigation,
        serviceName = constants.serviceName,
        selectedContainerId,
        selectedSubscriptionId;

    function onNavigateAway() {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Global.clear();
        Exp.UI.Commands.update();
    }

    function loadSettingsTab(extension, renderArea, renderData) {
        global.StorageSampleTenantExtension.SettingsTab.loadTab(renderData, renderArea);
    }

    function loadContainersTab(extension, renderArea, renderData) {
        global.StorageSampleTenantExtension.ContainersTab.loadTab(renderData, renderArea);
    }

    function loadStorageFilesTab(extension, renderArea, renderData) {
        var initData = {};
        initData.subscriptionId = selectedSubscriptionId;
        initData.containerId = selectedContainerId;

        global.StorageSampleTenantExtension.StorageFilesTab.loadTab(extension, renderArea, initData);
    }

    // This method is responsible for populating the left items seen when sub-tab is loaded.
    function loadContainersNavigationItemsDataFunction(data, originalPath, extension) {
        var items = $.map(global.StorageSampleTenantExtension.Controller.getContainersDataSet().data,
              function (value) {
                  return $.extend(value, {
                      name: value.ContainerId,  // This is the id of object.
                      displayName: value.ContainerName,
                      uniqueId: value.ContainerId,
                      navigationPath: {
                          type: value.Type,     // This is the type of object, you need to set this as a property in JSON data model.
                          name: value.ContainerId
                      }
                  });
              }
          );

        // Note: The following way of finding the subscription id for specific container id is not ideal.
        // This is done more as a hack. 
        var i, itemCount;
        for (i = 0, itemCount = items.length; i < itemCount; i++) {
            if (items[i] && items[i].ContainerId == selectedContainerId) {
                selectedSubscriptionId = items[i].SubscriptionId;
                break;
            }
        }

        return {
            data: items,
            backNavigation: {
                // This should be the id of the tab registered in navigation.
                // Note most of these matching are case-sensitive, yes **SENSITIVE**
                view: "containers" 
            }
        };
    }

    function onNavigating(context) {
        var destinationItem = context.destination.item;

        // We are navigating to drill downs for a container
        if (destinationItem) {
            if (destinationItem.type === "Container") { // This is the Type property value of JSON object.
                selectedContainerId = destinationItem.name;
            }
        }
    }

    navigation = {
        tabs: [
            {
                id: "containers",
                displayName: "containers",
                template: "ContainersTab",
                activated: loadContainersTab
            }
        ],
        types: [
            {
                name: "Container", // This is the type name of the object.
                dataFunction: loadContainersNavigationItemsDataFunction,
                tabs: [
                        {
                            id: "Files",
                            displayName: "files",
                            template: "StorageFilesTab",
                            activated: function (extension, renderArea, renderData) {
                                loadStorageFilesTab(extension, renderArea, renderData);
                            }
                        },
                ]
            }
        ]
    };

    global.StorageSampleTenantExtension = global.StorageSampleTenantExtension || {};

    StorageSampleTenantExtensionActivationInit = function () {
        var subs = Exp.Rdfe.getSubscriptionList(),
            subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService(serviceName),
            storageSampleExtension = $.extend(this, global.StorageSampleTenantExtension);

        // Don't activate the extension if user doesn't have a plan that includes the service.
        if (subscriptionRegisteredToService.length === 0) {
            return false; // Don't want to activate? Just bail
        }

        global.StorageSampleTenantExtension.Controller.getContainersDataSet(true);

        $.extend(storageSampleExtension, {
            viewModelUris: [storageSampleExtension.Controller.containerListUrl],
            displayName: constants.serviceDisplayName,
            navigationalViewModelUri: {
                uri: storageSampleExtension.Controller.containerListUrl,
                ajaxData: function () {
                    return global.Exp.Rdfe.getSubscriptionIdsRegisteredToService(serviceName);
                }
            },
            displayStatus: global.waz.interaction.statusIconHelper(global.StorageSampleTenantExtension.ContainersTab.statusIcons, "Status"),
            menuItems: [
                {
                    name: "StorageSampleMenuItem",
                    displayName: "Storage Sample",
                    url: "#Workspaces/StorageSampleTenantExtension",
                    preview: "createPreview",
                    isEnabled: function () {
                        return {
                            enabled: true,
                            description: "Create data storage services"
                        }
                    },
                    subMenu: [
                        getQuickCreateContainerMenuItem()
                    ]
                }
            ],
            getResources: function () {
                return resources;
            }
        });

        storageSampleExtension.onNavigating = onNavigating;
        storageSampleExtension.onNavigateAway = onNavigateAway;
        storageSampleExtension.navigation = navigation;

        Shell.UI.Pivots.registerExtension(storageSampleExtension, function () {
            Exp.Navigation.initializePivots(this, navigation);
        });

        // Need to register types, so that navigation to sub-levels can be enabled for tabs.
        Exp.TypeRegistry.add(storageSampleExtension.name, navigation.types);

        // Finally activate and give "the" storageSampleExtension the activated extension since a good bit of code depends on it
        $.extend(global.StorageSampleTenantExtension, Shell.Extensions.activate(storageSampleExtension));
    };

    function getQuickCreateContainerMenuItem() {
        return {
            name: "QuickCreateContainer",
            displayName: "Create Container",
            description: "Create new container",
            template: "ContainerQuickCreateMenu",
            label: "CREATE",

            opening: function () {
            },

            open: function () {
                var promise = controller.getLocationsAsync();
                promise.done(function (response) {
                    var listOfLocations = response.data;
                    var locationDropDown = $('#hw-qc-share-server');
                    var options = $.templates("<option value=\"{{>LocationId}}\">{{>LocationName}}</option>").render(listOfLocations);
                    locationDropDown.append(options);
                });

                // TODO: Ideally if there is only one subscription, user shouldn't be prompted.
                var subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService(serviceName);
                var subscriptionDropDown = $('#hw-qc-container-subscription');
                var subscriptionOptions = $.templates("<option value=\"{{>id}}\">{{>OfferFriendlyName}}</option>").render(subscriptionRegisteredToService);
                subscriptionDropDown.append(subscriptionOptions);
            },

            ok: function (object) {
                var name = object.fields['containerName'];
                var promise = controller.createContainerAsync(object.fields['selectedSubscription'], name, object.fields['selectedLocation']);
                global.waz.interaction.showProgress(
                    promise,
                    {
                        initialText: "Creating container '" + name + "'.",
                        successText: "Successfully created container '" + name + "'.",
                        failureText: "Failed to create container '" + name + "'."
                    }
                );

                promise.done(function () {
                    StorageSampleTenantExtension.Controller.getContainersDataSet(true);
                    return true;
                });
                promise.fail(function () {
                    return false;
                });
            },

            cancel: function (dialogFields) {
                // you can return false to cancel the closing
            }
        };
    }

    Shell.Namespace.define("StorageSampleTenantExtension", {
        serviceName: serviceName,
        init: StorageSampleTenantExtensionActivationInit,
        getQuickCreateContainerMenuItem: getQuickCreateContainerMenuItem
    });
})(jQuery, this);
