/*globals window,jQuery,cdm,StorageSampleTenantExtension,waz,Exp*/
(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/StorageSampleTenant",
        folderListUrl = baseUrl + "/ListFolders",
        constants = global.StorageSampleTenantExtension.Constants;

    function getFoldersDataSet(triggerRefreshing) {
        if (triggerRefreshing) {
            Exp.Data.getData({
                url: folderListUrl,
                ajaxData: {
                    subscriptionIds: Exp.Rdfe.getSubscriptionIdsRegisteredToService(constants.serviceName).subscriptionIds
                },
                forceCacheRefresh: true
            });
        }

        return Exp.Data.getLocalDataSet(folderListUrl);
    }

    function createFolderAsync(subscriptionId, folderName, shareId) {
        var data = {};
        data.subscriptionId = subscriptionId;

        data.folder = {};
        data.folder.FolderName = folderName;
        data.folder.ShareId = shareId;

        return Shell.Net.ajaxPost({
            data: data,
            url: baseUrl + "/CreateFolder"
        });
    }

    function deleteFolderAsync(subscriptionId, folderId) {
        return Shell.Net.ajaxPost({
                data: {
                    subscriptionId: subscriptionId,
                    folderId: folderId
                },
                url: baseUrl + "/DeleteFolder"
            });
    }

    function getSharesAsync() {
        var subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService(constants.serviceName);
        return Shell.Net.ajaxPost({
            url: baseUrl + "/ListShares",
            data: {
                subscriptionIds: subscriptionRegisteredToService[0].id
            }
        });
    }

    function getStorageFilesAsync(subscriptionId, folderId) {
        return Shell.Net.ajaxPost({
            data: {
                subscriptionId: subscriptionId,
                folderId: folderId
            },
            url: baseUrl + "/ListStorageFiles"
        });
    }

    function uploadFileAsync(formSelector) {
        var _deferred = $.Deferred();
        global.AppExtension.UI.fileUpload({
            formSelector: formSelector,
            url: baseUrl + "/UploadStorageFile",
            onComplete: function (result) {
                _deferred.resolve(result);
            },
            onError: function (result) {
                _deferred.reject(result);
            }
        });
        return _deferred.promise();
    }

    global.StorageSampleTenantExtension = global.StorageSampleTenantExtension || {};
    global.StorageSampleTenantExtension.Controller = {
        getFoldersDataSet: getFoldersDataSet,
        createFolderAsync: createFolderAsync,
        deleteFolderAsync: deleteFolderAsync,
        getSharesAsync: getSharesAsync,
        getStorageFilesAsync: getStorageFilesAsync,
        uploadFileAsync: uploadFileAsync
    };
})(jQuery, this);
