using System;
using System.Collections.Generic;
using Neotys.DesignAPI.Client;
using Neotys.DesignAPI.Model;
namespace wsTransferToNeoLoad
{
    /// <summary>
    /// Action Handler for CertifyClass "TransferToNeoLoadClass"
    /// Handles the following actions:
    ///     "StartRecordingAction"
    /// </summary>
    ///
    /// <remarks>
    ///
    /// </remarks> 
    public class TransferToNeoLoadClassActions : StepAction
    {
        // Map Certify Action names to method action handlers
        private static readonly Dictionary<string, Func<ProcessStepData, ActionResult>> ActionMap =
            new Dictionary<string, Func<ProcessStepData, ActionResult>>();

        LoggingService _log = LoggingService.GetLogger;

        NeoLoadDesignApiInstance neoLoadDesignApiInstance = null;

        public TransferToNeoLoadClassActions()
        {
            // Manually add an action handler
            ActionMap.Add("StartRecordingAction", data => { return StartRecordingActionHandler(data); });
            ActionMap.Add("StopRecordingAction", data => { return StopRecordingActionHandler(data); });            
        }

        public override ActionResult Execute(ProcessStepData stepData)
        {
            Func<ProcessStepData, ActionResult> actionHandler;
            if (ActionMap.TryGetValue(stepData.Action, out actionHandler) == false)
            {
                actionHandler = ActionMap[$"{stepData.Action}Handler"];
            }

            if (actionHandler != null)
            {
                return actionHandler.Invoke(stepData);
            }

            return new ActionResult(false, $"No action handler defined for action {stepData.Action}", "");
        }

        public ActionResult StartRecordingActionHandler(ProcessStepData stepData)
        {
            _log.Info("Starting execution of StartRecordingActionHandler");

            string apiKey = stepData.GetActionArg(StartRecordingParameters.API_KEY, "");
            string url = stepData.GetActionArg(StartRecordingParameters.DESIGN_API_URL, "http://localhost:7400/Design/v1/Service.svc/");
            string userPath = stepData.GetActionArg(StartRecordingParameters.USER_PATH, "");

            _log.Info("Connecting to NeoLoad Design API");
            neoLoadDesignApiInstance = new NeoLoadDesignApiInstance(url, apiKey);

            if(userPath != null && userPath.Length != 0)
            {
                neoLoadDesignApiInstance.SetUserPathName(userPath);
            }
           
            string message;
            bool status;
            try
            {
                _log.Info("Sending API call StartRecording");
                neoLoadDesignApiInstance.StartSapRecording();
                message = "record started";
                status = true;
            }
            catch (Exception e)
            {
                _log.Error("Unable to send API call StartRecording", e);
                status = false;
                message = e.Message;
            }

            return new ActionResult(status, message, "");
        }

        public ActionResult StopRecordingActionHandler(ProcessStepData stepData)
        {
            _log.Info("Starting execution of StopRecordingActionHandler");

            if(neoLoadDesignApiInstance == null || !neoLoadDesignApiInstance.IsRecordStarted())
            {
                _log.Info("Recording not started, nothing to stop.");
                return new ActionResult(false, "No recording started", "");
            }

            string message;
            bool status;
            try
            {
                _log.Info("Sending API call StopRecording");
                neoLoadDesignApiInstance.StopRecording();
                message = "record stopped";
                status = true;
            }
            catch (Exception e)
            {
                _log.Error("Unable to send API call StopRecording", e);
                status = false;
                message = e.Message;
            }

            return new ActionResult(status, message, "");
        }
    }
}