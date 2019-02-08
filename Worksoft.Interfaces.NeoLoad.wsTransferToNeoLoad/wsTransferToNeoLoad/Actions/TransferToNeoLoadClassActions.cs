using System;
using System.Collections.Generic;
namespace wsNeoLoad
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
            ActionMap.Add("StartRecording", data => { return StartRecordingActionHandler(data); });
            ActionMap.Add("StopRecording", data => { return StopRecordingActionHandler(data); });            
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
            string recordMode = stepData.GetActionArg(StartRecordingParameters.RECORD_MODE, "");
            string userPath = stepData.GetActionArg(StartRecordingParameters.USER_PATH, "");
            string advanced = stepData.GetActionArg(StartRecordingParameters.ADVANCED, "");

            AdvancedParameters advancedParameters = new AdvancedParameters(advanced);
            string url = advancedParameters.GetValue(StartRecordingParameters.DESIGN_API_URL, "http://localhost:7400/Design/v1/Service.svc/");
           
            string addressToExclude = advancedParameters.GetValue(StartRecordingParameters.ADDRESS_TO_EXCLUDE, "");
           
            string message;
            bool status;
            try
            {
                _log.Info("Connecting to NeoLoad Design API");
                neoLoadDesignApiInstance = new NeoLoadDesignApiInstance(url, apiKey);
                SystemProxyHelper systemProxyHelper = null;

                if (userPath != null && userPath.Length != 0)
                {
                    neoLoadDesignApiInstance.SetUserPathName(userPath);
                }

                String recorderProxyHost = neoLoadDesignApiInstance.GetRecorderProxyHost();
                int recorderProxyPort = neoLoadDesignApiInstance.GetRecorderProxyPort();
                _log.Info("Recorder proxy (host:port) " + recorderProxyHost + ":" + recorderProxyPort);

                int apiPort = neoLoadDesignApiInstance.GetApiPort();
                systemProxyHelper = new SystemProxyHelper(apiPort);

                _log.Info("Setting system proxy");
                // TODO systemProxyHelper.setProxy(recorderProxyHost, recorderProxyPort, addressToExclude);
                
                _log.Info("Sending API call StartRecording");
                neoLoadDesignApiInstance.StartRecording(recordMode);
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

                _log.Info("Restoring system proxy");
                // TODO systemProxyHelper.restoreProxy();

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