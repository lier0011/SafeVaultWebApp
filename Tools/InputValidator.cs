using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace SafeVaultWebApp.Tools;

public static class InputValidator
{
    public static bool IsValidInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        // Check for HTML tags
        if (Regex.IsMatch(input, @"<.*?>"))
        {
            return false;
        }

        // Check for SQL keywords or patterns
        string[] sqlKeywords = { "SELECT", "INSERT", "DELETE", "UPDATE", "DROP", "--", ";" };
        foreach (var keyword in sqlKeywords)
        {
            if (Regex.IsMatch(input, $@"\b{keyword}\b", RegexOptions.IgnoreCase))
            {
                return false;
            }
        }

        // Reject input with special characters often used in attacks
        if (Regex.IsMatch(input, @"['"";]"))
        {
            return false;
        }

        return true;
    }
}