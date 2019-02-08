using System;
using System.Collections.Generic;

namespace wsTransferToNeoLoad
{
    public static class StepActions
    {
        private static readonly LoggingService _log = LoggingService.GetLogger;
        // Map of CertifyClass types to Action Handler classes
        private static readonly Dictionary<string, StepAction> ClassMap = new Dictionary<string, StepAction>();

        static StepActions()
        {
            ClassMap.Add(@"TransferToNeoLoadClass", new TransferToNeoLoadClassActions());
        }

        // Process the requested action
        public static ActionResult RunAction(ProcessStepData stepData)
        {
            ActionResult result;
            try
            {
                var action = ClassMap[stepData.CertifyClass];
                result = action.Execute(stepData);
            }
            catch (Exception e)
            {
               _log.Error($"Unable to process action for data {stepData}", e); 
                result = new ActionResult(false, $"No Class handler defined for Class {stepData.CertifyClass}", "");
            }

            return result;
        }
    }
}