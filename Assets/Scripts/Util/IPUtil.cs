using System.Text.RegularExpressions;
using UnityEngine;

public static class IPUtil
{
    public static bool IsValidIPv4(string ipAddress)
    {
        string pattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        return Regex.IsMatch(ipAddress, pattern);
    }
}
