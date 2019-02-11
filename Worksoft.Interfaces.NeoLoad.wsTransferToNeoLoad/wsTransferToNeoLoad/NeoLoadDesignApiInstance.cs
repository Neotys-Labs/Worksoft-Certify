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

        private string _recorderProxyHost = null;
        private int _recorderProxyPort = 0;
        private int _apiPort = 0;

        public NeoLoadDesignApiInstance(string url, string apiKey)
        {
            _client = DesignAPIClientFactory.NewClient(url, apiKey);
            _recorderProxyHost = ExtractHost(url);
            _recorderProxyPort = _client.GetRecorderSettings().ProxySettings.Port;
            _apiPort = ExtractPort(url);
        }

        public void SetUserPathName(string name)
        {
            _userPathName = name;
        }

        public bool IsRecordStarted()
        {
            return _recordStarted;
        }

        public void StartRecording(string recordMode, string userAgent, bool isHttp2)
        {
            _recordStarted = true;
            StartRecordingParamsBuilder startRecordingPB = new StartRecordingParamsBuilder();
            HandleUserPathName(startRecordingPB);

            bool isSap = recordMode.ToLower().Contains("sap");
            startRecordingPB.isSapGuiProtocol(isSap);
            startRecordingPB.isHTTP2Protocol(isHttp2);
            startRecordingPB.userAgent(userAgent);

            try
            {
                _client.StartRecording(startRecordingPB.Build());
            }
            catch (Exception e)
            {
                _recordStarted = false;
                throw e;
            }
        }

        private void HandleUserPathName(StartRecordingParamsBuilder startRecordingPB)
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
                        startRecordingPB.virtualUser(_userPathName + "_recording");
                    }
                    else
                    {
                        startRecordingPB.virtualUser(_userPathName);
                    }
                }
                else
                {
                    startRecordingPB.virtualUser(_userPathName);
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
            return _recorderProxyHost;
        }

        public int GetRecorderProxyPort()
        {
            return _recorderProxyPort;
        }

        public int GetApiPort()
        {
            return _apiPort;
        }

        private static string ExtractHost(string url)
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

        public void SetUpdateUserPath(bool updateUserPath)
        {
            _updateUserPath = updateUserPath;


        }

        private static int ExtractPort(string url)
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

        public void StartTransaction(string transactionName)
        {
            _client.SetContainer(new SetContainerParams(transactionName));
        }
    }
}
