using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace wsNeoLoad
{
    public class AdvancedParameters
    {
        Dictionary<string, string> data;

        public AdvancedParameters(string value)
        {
            data = new Dictionary<string, string>();
            if (value != null && value.Length != 0)
            {
                foreach (var row in Regex.Split(value, "\r\n|\r|\n"))
                    data.Add(row.Split('=')[0], row.Split('=')[1]);
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

    }
}
