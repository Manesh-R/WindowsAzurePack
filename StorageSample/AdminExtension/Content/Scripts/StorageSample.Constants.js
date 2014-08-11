/// <reference path="../../scripts/_references.js" />
/*globals */
/// <dictionary>runbook,runbooks,Runbooks,datetime,quickstart,url</dictionary>

(function ($, global, shell, undefined) {
    "use strict";

    shell.Namespace.define("StorageSampleAdminExtension.Constants", {
        extensionName: "StorageSampleAdminExtension",
        TypeNames: {
            runbook: "Runbook",
            job: "Job",
            stream: "Stream",
            module: "Module",
            activity: "Activity",
            credential: "Credential",
            variable: "Variable",
            connection: "Connection",
            certificate: "Certificate",
            schedule: "Schedule",
            dailySchedule: "DailySchedule",
            oneTimeSchedule: "OneTimeSchedule",
            runbookSchedule: "RunbookSchedule",
            unKnown: "Unknown",
            getAssetTypes: function() {
                return [this.module, this.activity, this.credential, this.variable, this.connection, this.certificate, this.schedule, this.dailySchedule, this.oneTimeSchedule];
            },
            runbookConfiguration: "RunbookConfiguration"
        },
        RegEx: {
            twentyFourHourTime: /^\s*(\d{1,2})\:(\d{1,2})$\s*/,
            url: /\^http[\-a-zA-Z0-9@:%_\+.~#?&\/\/=]{2,256}\.[a-z]{2,4}\b(\/[\-a-zA-Z0-9@:%_\+.~#?&\/\/=]*)?\^/
        },
        EncryptedValue: {
            defaultPasswordPlaceholder: "!###############!"
        },
        RunbookStatus: {
            // These values must match the status values coming from the service for the Runbook type, including casing
            available: "Available",
            offline: "Offline",
            unknown: "Unknown"
        },
        Commands: {
            startRunbookCommandId: "StartRunbook",
            importRunbookCommandId: "ImportRunbook",
            exportRunbookCommandId: "ExportRunbook",
            deleteRunbookCommandId: "DeleteRunbook",
            editRunbookCommandId: "EditRunbook",
            testDraftRunbookCommandId: "TestDraftRunbook",
            publishDraftRunbookCommandId: "PublishDraftRunbook",
            insertAuthoringArtifactCommandId: "InsertAuthoringArtifact",
            insertActivityAuthoringArtifactCommandId: "InsertActivityAuthoringArtifact",
            insertRunbookAuthoringArtifactCommandId: "InsertRunbookAuthoringArtifact",
            insertSettingAuthoringArtifactCommandId: "InsertSettingAuthoringArtifact",
            importModuleCommandId: "ImportModule",
            deleteModuleCommandId: "DeleteModule",
            newSettingCommandId: "NewSetting",
            editSettingCommandId: "EditSetting",
            deleteSettingCommandId: "DeleteSetting",
            suspendJobCommandId: "SuspendJob",
            stopJobCommandId: "StopJob",
            resumeJobCommandId: "ResumeJob",
            saveCommandId: "Save",
            addAssetCommandId: "AddAsset",
            deleteAssetCommandId: "DeleteAsset",
            viewStreamDetailsCommandId: "ViewStreamDetails",
            viewRunbookSourceCommandId: "ViewRunbookSource",
            discardCommandId: "Discard",
            manageCommandId: "Manage",
            scheduleCommandId: "Schedule",
            addNewScheduleCommandId: "AddNewSchedule",
            useExistingScheduleCommandId: "UseExistingSchedule",
            viewScheduleDetailsCommandId : "ViewScheduleDetails",
            removeScheduleFromRunbookCommandId: "removeSchedule"
        },
        NotificationType: {
            error: "error",
            warning: "warning",
            information: "information"
        },
        SupportedParameterTypes: {
            datetime: (function () { return "System.DateTime".toLowerCase(); })(),
            string: (function () { return "System.String".toLowerCase(); })(),
            boolean: (function () { return "System.Boolean".toLowerCase(); })(),
            unsignedInteger: (function () { return "System.UInt32".toLowerCase(); })(),
            integer: (function () { return "System.Int32".toLowerCase(); })(),
            // switch is a reserved term, and using it as "switch" causes build errors
            switchParameter: (function () { return "System.Management.Automation.SwitchParameter".toLowerCase(); })()
        },
        TabNames: {
            quickstart: "quickstart",
            dashboard: "dashboard"
        },
        // TODO: use the appropriate user settings for automation
        UserSettings: {
            source: "StorageSampleAdminExtension",
            type: "User",
            subscriptionId: "",
            pathAllRunbooks: "Runbooks",
            pathRunbook: "Runbooks/{0}",
            resourceName: "Metrics"
        },
        DefaultDates: {
            jobFilterStartDate: (function() {
                 return new Date(2013, 0, 1);
            })()
        },
        JobStates: {
            newJob: "New", // can't use "new" because it is reserved
            activating: "Activating",
            activationFailed: "ActivationFailed",
            running: "Running",
            stopping: "Stopping",
            stopped: "Stopped",
            suspending: "Suspending",
            suspended: "Suspended",
            resuming: "Resuming",
            failed: "Failed",
            completed: "Completed"
        },
        JobTrackingFailedStates: {
            initialization: "initialization",
            unexpected: "unexpected",
            failed: "failed"
        },
        StreamType: {
            progress: "Progress",
            output: "Output",
            debug: "Debug",
            verbose: "Verbose",
            error: "Error",
            warning: "Warning"
        },
        DashboardStatusOrder: {
            queued: "1",
            failed: "2",
            stopped: "3",
            suspended: "4",
            completed: "5",
            running: "6"
        }
    });

})(jQuery, this, Shell);