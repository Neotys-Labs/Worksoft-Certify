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

        public bool GetActionArg(string parameterName, ref string value, ref string errorMsg)
        {
//            object[] args = { parameterName ?? string.Empty, value ?? string.Empty, errorMsg ?? string.Empty };
//            ParameterModifier[] paramMod = { new ParameterModifier(3) };
//            paramMod[0][0] = true;
//            paramMod[0][1] = true;
//            paramMod[0][2] = true;

            var retVal = (int) GetWsPerform().GetActionArg(ref parameterName, ref value, ref errorMsg);
//                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
//                null, wsLib, args, paramMod, null, null);

//            parameterName = args[0] as string;
//            value = args[1] as string;
//            errorMsg = args[2] as string;

            _log.Debug($"Get Argument {parameterName}={value}");
            return retVal != 0;
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