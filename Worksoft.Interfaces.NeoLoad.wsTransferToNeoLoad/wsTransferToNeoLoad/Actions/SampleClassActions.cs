using System;
using System.Collections.Generic;
using Neotys.DesignAPI.Client;
using Neotys.DesignAPI.Model;
namespace wsTransferToNeoLoad
{
    /// <summary>
    /// Action Handler for CertifyClass "SampleClass"
    /// Handles the following actions:
    ///     "SampleAction",
    ///     "Add"
    /// </summary>
    ///
    /// <remarks>
    ///
    /// </remarks> 
    public class SampleClassActions : StepAction
    {
        // Map Certify Action names to method action handlers
        private static readonly Dictionary<string, Func<ProcessStepData, ActionResult>> ActionMap =
            new Dictionary<string, Func<ProcessStepData, ActionResult>>();

        LoggingService _log = LoggingService.GetLogger;

        public SampleClassActions()
        {
//            actionMap.Add("SampleActionHandler", delegate(ProcessStepData data) { return SampleActionHandler(data); });

            // Manually add an action handler
            ActionMap.Add("SampleAction", data => { return SampleActionHandler(data); });
            ActionMap.Add("Add", data => { return AddHandler(data); });

            // or Automagically add all methods matching a given signature as action handlers
            // finds all methods defined on this class with the signature of taking an ProcessStepData arg and returning a ActionResult
//            foreach (var method in GetType().GetMethods())
//            {
//                if (!method.IsStatic && method.ReturnType == typeof(ActionResult)
//                                     && method.GetParameters().Length == 1
//                                     && method.GetParameters()[0].ParameterType == typeof(ProcessStepData)
//                                     && method.GetBaseDefinition().DeclaringType == method.DeclaringType)
//                {
//                    var m =
//                        (Func<ProcessStepData, ActionResult>) method.CreateDelegate(typeof(Func<ProcessStepData, ActionResult>));
//
//                    ActionMap.Add(method.Name, m);
//                }
//            }
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

        public ActionResult SampleActionHandler(ProcessStepData stepData)
        {
            _log.Info("Starting execution of SampleActionHandler");
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

        public ActionResult AddHandler(ProcessStepData stepData)
        {
            string result = string.Empty;
            string msg = string.Empty;
            string left = null;
            string right = null;
            string error = null;
            stepData.GetActionArg("left", ref left, ref error);
            stepData.GetActionArg("right", ref right, ref error);

            if (!string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(right))
            {
                if (Int32.TryParse(left, out var l) && Int32.TryParse(right, out var r))
                {
                    result = $"{l + r}";
                }
                else
                {
                    result = left + right;
                }
            }
            else
            {
                msg = "Unable to get action arguments";
            }

            
            return new ActionResult(true, msg, result);
        }
    }
}