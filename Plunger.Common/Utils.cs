using System.Security.Cryptography;
using System.Text;
using Geralt;

namespace Plunger.Common;

public class Utils
{
    public static string GetEnvironmentVariable(string varName)
    {
        var envVar = Environment.GetEnvironmentVariable(varName);
        if (envVar == null)
        {
            throw new Exception("No environment variable named: " + varName);
        }

        return envVar;
    }

    public static string BuildQueryUrl(Dictionary<string, string> queryParams)
    {
        var queryString = string.Join('&', queryParams.Select(kvp => kvp.Key + '=' + kvp.Value));
        return '?' + queryString;
    }
}