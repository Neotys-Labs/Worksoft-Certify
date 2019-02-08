using System;
using Neotys.DesignAPI.Client;
using Neotys.DesignAPI.Model;

namespace wsNeoLoad
{
    class NeoLoadDesignApiInstance
    {

        private IDesignAPIClient _client = null;
        private string _userPathName = null;
        private bool _userPathExist = false;
        private bool _recordStarted = false;
        private string recorderProxyHost = null;
        private int recorderProxyPort = 0;

        public NeoLoadDesignApiInstance(string url, string apiKey)
        {
            _client = DesignAPIClientFactory.NewClient(url, apiKey);
            recorderProxyHost = extractHost(url);
            recorderProxyPort = _client.GetRecorderSettings().ProxySettings.Port;
        }

        public void SetUserPathName(string name)
        {
            _userPathName = name;
        }

        public bool IsRecordStarted()
        {
            return _recordStarted;
        }

        public void StartRecording(string recordMode)
        {
            _recordStarted = true;
            StartRecordingParamsBuilder _startRecordingPB = new StartRecordingParamsBuilder();
            if (_userPathName != null && _userPathName.Length != 0)
            {
                ContainsUserPathParamsBuilder _containsBuilder = new ContainsUserPathParamsBuilder();
                _containsBuilder.name(_userPathName);
                _userPathExist = _client.ContainsUserPath(_containsBuilder.Build());
                if (_userPathExist)
                {
                    _startRecordingPB.virtualUser(_userPathName + "_recording");
                }
                else
                {
                    _startRecordingPB.virtualUser(_userPathName);
                }
            }

            bool isSap = recordMode.ToLower().Contains("sap");
            _startRecordingPB.isSapGuiProtocol(isSap);

            try
            {
                _client.StartRecording(_startRecordingPB.Build());
            }
            catch (Exception e)
            {
                _recordStarted = false;
                throw e;
            }
        }

        public void StopRecording()
        {
            StopRecordingParamsBuilder _stopRecordingBuilder = new StopRecordingParamsBuilder();

            if (_userPathExist)
            {
                UpdateUserPathParamsBuilder _updateUserPathBuilder = new UpdateUserPathParamsBuilder();
                _updateUserPathBuilder.name(_userPathName);
                _updateUserPathBuilder.deleteRecording(true);
                _stopRecordingBuilder.updateParams(_updateUserPathBuilder.Build());
            }

            try
            {
                _client.StopRecording(_stopRecordingBuilder.Build());
            }
            finally
            {
                _recordStarted = false;
            }
        }

        public int GetRecorderProxyPort()
        {
            return recorderProxyPort;
        }

        public string GetRecorderProxyHost()
        {
            return recorderProxyHost;
        }

        private static string extractHost(string url)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
                String domain = uri.Host;
                return domain.StartsWith("www.") ? domain.Substring(4) : domain;
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.ToString());
                return "localhost";
            }
        }
    }
}
