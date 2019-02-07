using System.Runtime.InteropServices;

namespace wsTransferToNeoLoad
{
    /// <summary>
    /// Main Entry point for Handling Certify Steps
    /// 
    /// </summary>
    [ComVisible(true)]
    [Guid("68E24F80-A2F1-451E-B988-039A0B185078")] 
    public class Main
    {
        private static readonly ILoggingService _log = LoggingService.GetLogger;

        public short Dispatcher(
            string certifyClass,
            string action,
            string windowAttribute,
            string lastWindow,
            string windowPhysicalName,
            string objectAttribute,
            string objectPhysicalName,
            string expectedValue,
            ref string actualValue,
            ref string errorMessage,
            ref int isFatal)
        {
            _log.DebugFormat("Invoking action {0}", action);
            var stepData = new ProcessStepData
            {
                CertifyClass = certifyClass,
                Action = action,
                WindowAttribute = windowAttribute,
                LastWindow = lastWindow,
                WindowPhysicalName = windowPhysicalName,
                ObjectAttribute = objectAttribute,
                ObjectPhysicalName = objectPhysicalName,
                ExpectedValue = expectedValue,
                ActualValue = actualValue,
                ErrorMessage = errorMessage,
                IsFatal = isFatal
            };

            // perform the requested action
            var result = StepActions.RunAction(stepData);

            _log.DebugFormat("Action result: {0}", result.Message);

            // set values to return to Certify
            actualValue = result.Value;
            errorMessage = result.Message;
            return (short) (result.Status ? DispatcherResult.PASSED : DispatcherResult.FAILED);
        }
    }
}