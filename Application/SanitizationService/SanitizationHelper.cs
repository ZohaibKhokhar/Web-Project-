using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.ServiceInterfaces;

namespace Application.SanitizationService
{
    public class SanitizationHelper : ISanitizationHelper
    {
        public async Task<string> SanitizeString(string input)
        {
            // Run the synchronous sanitization code on a background thread
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(input))
                    return input;

                // Strip out dangerous characters
                string sanitized = Regex.Replace(input, @"[<>\@|();?{}\[\]]", string.Empty);

                // Encode HTML characters to prevent XSS
                sanitized = System.Net.WebUtility.HtmlEncode(sanitized);

                return sanitized;
            });
        }
    }
}
