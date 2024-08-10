using System.Text.RegularExpressions;
using Domain.ServiceInterfaces;
namespace Application.SanitizationService
{

    public class SanitizationHelper : ISanitizationHelper
    {
        public string SanitizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Example: Strip out dangerous characters
            string sanitized = Regex.Replace(input, @"[<>\@|();?{}\[\]]", string.Empty);

            // Example: Encode HTML characters to prevent XSS
            sanitized = System.Net.WebUtility.HtmlEncode(sanitized);

            return sanitized;
        }
    }

}
