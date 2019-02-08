using System;

namespace wsTransferToNeoLoad
{
    public class ProcessStepData
    {
        private readonly LoggingService _log = LoggingService.GetLogger;
        private dynamic wsLib;

        public string CertifyClass { get; set; }

        public string Action { get; set; }

        public string WindowAttribute { get; set; }

        public string LastWindow { get; set; }

        public string WindowPhysicalName { get; set; }

        public string ObjectAttribute { get; set; }

        public string ObjectPhysicalName { get; set; }

        public string ExpectedValue { get; set; }

        public string ActualValue { get; set; }

        public string ErrorMessage { get; set; }

        public int IsFatal { get; set; }

        public string GetActionArg(string parameterName, string defaultValue)
        {
            string value = defaultValue;
            string errorMsg = "";
            var retVal = (int) GetWsPerform().GetActionArg(ref parameterName, ref value, ref errorMsg);

            _log.Debug($"Get Argument {parameterName}={value}");

            return retVal != 0 ? value : defaultValue;
        }


        private dynamic GetWsPerform()
        {
            if (wsLib == null)
            {
                try
                {
                    var performType = Type.GetTypeFromProgID("wsLib.Perform");
                    wsLib = Activator.CreateInstance(performType);
                }
                catch (Exception e)
                {
                    _log.Error("Unable to initialize wsLib.Perform.  Communications with Certify cannot be established.", e);
                    throw;
                }
            }

            return wsLib;
        }

        public override string ToString()
        {
            return $"{nameof(wsLib)}: {wsLib}, {nameof(CertifyClass)}: {CertifyClass}, {nameof(Action)}: {Action}, {nameof(WindowAttribute)}: {WindowAttribute}, {nameof(LastWindow)}: {LastWindow}, {nameof(WindowPhysicalName)}: {WindowPhysicalName}, {nameof(ObjectAttribute)}: {ObjectAttribute}, {nameof(ObjectPhysicalName)}: {ObjectPhysicalName}, {nameof(ExpectedValue)}: {ExpectedValue}, {nameof(ActualValue)}: {ActualValue}, {nameof(ErrorMessage)}: {ErrorMessage}, {nameof(IsFatal)}: {IsFatal}";
        }
    }
}