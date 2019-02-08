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
            string apiKey = "";
            string error = "";
            stepData.GetActionArg("ApiKey", ref apiKey, ref error);
            
            string url = "http://localhost:7400/Design/v1/Service.svc/";
            _log.Info("Connecting to NeoLoad Design API");
            IDesignAPIClient _client = DesignAPIClientFactory.NewClient(url, apiKey);
            
            StartRecordingParamsBuilder _startRecordingPB = new StartRecordingParamsBuilder();

            string message;
            bool status;
            try
            {
                _log.Info("Sending API call StartRecording");
                _client.StartRecording(_startRecordingPB.Build());
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
            string apiKey = "";
            string error = "";
            stepData.GetActionArg("ApiKey", ref apiKey, ref error);

            string url = "http://localhost:7400/Design/v1/Service.svc/";
            _log.Info("Connecting to NeoLoad Design API");
            // TODO retrieve client from singleton
            IDesignAPIClient _client = DesignAPIClientFactory.NewClient(url, apiKey);

            StopRecordingParamsBuilder _stopRecordingPB = new StopRecordingParamsBuilder();

            string message;
            bool status;
            try
            {
                _log.Info("Sending API call StopRecording");
                _client.StopRecording(_stopRecordingPB.Build());
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