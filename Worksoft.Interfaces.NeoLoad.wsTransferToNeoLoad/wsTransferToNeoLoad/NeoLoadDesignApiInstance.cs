using System;
using Neotys.DesignAPI.Client;
using Neotys.DesignAPI.Model;

namespace wsNeoLoad
{
    class NeoLoadDesignApiInstance
    {

        private IDesignAPIClient _client = null;
        private bool _updateUserPath = true;
        private string _userPathName = null;
        private bool _userPathExist = false;
        private bool _recordStarted = false;

        private string recorderProxyHost = null;
        private int recorderProxyPort = 0;
        private int apiPort = 0;

        public NeoLoadDesignApiInstance(string url, string apiKey)
        {
            _client = DesignAPIClientFactory.NewClient(url, apiKey);
            recorderProxyHost = extractHost(url);
            recorderProxyPort = _client.GetRecorderSettings().ProxySettings.Port;
            apiPort = extractPort(url);
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
            handleUserPathName(_startRecordingPB);

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

        private void handleUserPathName(StartRecordingParamsBuilder _startRecordingPB)
        {
            if (_userPathName != null && _userPathName.Length != 0)
            {
                if (_updateUserPath)
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
                else
                {
                    _startRecordingPB.virtualUser(_userPathName);
                }
            }
        }

        public void StopRecording(StopRecordingParamsBuilder stopRecordingBuilder, UpdateUserPathParamsBuilder updateUserPathBuilder)
        {
            if (_userPathExist)
            {
                updateUserPathBuilder.name(_userPathName);
                stopRecordingBuilder.updateParams(updateUserPathBuilder.Build());
            }

            try
            {
                _client.StopRecording(stopRecordingBuilder.Build());
            }
            finally
            {
                _recordStarted = false;
            }
        }

        public string GetRecorderProxyHost()
        {
            return recorderProxyHost;
        }

        public int GetRecorderProxyPort()
        {
            return recorderProxyPort;
        }

        public int GetApiPort()
        {
            return apiPort;
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

        internal void setUpdateUserPath(bool updateUserPath)
        {
            _updateUserPath = updateUserPath;


        }

        private static int extractPort(string url)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
                return uri.Port;
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.ToString());
                return 7400;
            }
        }
    }
}
