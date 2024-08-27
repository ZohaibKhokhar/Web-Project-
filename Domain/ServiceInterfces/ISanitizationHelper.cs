namespace Domain.ServiceInterfaces
{
    public interface ISanitizationHelper
    {
        Task<string> SanitizeString(string input);
    }
}
