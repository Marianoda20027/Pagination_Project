namespace PaginationApp.Core.Exceptions
{
    public class ConfigurationException : AppException
    {
        public ConfigurationException(string message) 
            : base(message, 500) { }
    }
}
