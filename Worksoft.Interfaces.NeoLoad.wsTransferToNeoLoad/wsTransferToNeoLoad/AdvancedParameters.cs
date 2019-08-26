using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace wsNeoLoad
{
    public class AdvancedParameters
    {
        private static readonly LoggingService _log = LoggingService.GetLogger;

        Dictionary<string, string> data;

        public AdvancedParameters(string value)
        {
            data = new Dictionary<string, string>();
            if (value != null && value.Length != 0)
            {
                foreach (var row in Regex.Split(value, "\r\n|\r|\n"))
                {
                    if(row.Contains("="))
                    {
                        data.Add(row.Split('=')[0], row.Split('=')[1]);
                    } else
                    {
                        _log.Error("Value " + row + " does not have right format.");
                    }
                }
            }
        }

        public string GetValue(string parameterName, string defaultValue)
        {
            if (data.ContainsKey(parameterName))
            {
                return data[parameterName];
            }
            return defaultValue;
        }

        public bool GetBooleanValue(string parameterName, string defaultValue)
        {
            return GetValue(parameterName, defaultValue).ToLower().Equals("true");
        }

    }
}
