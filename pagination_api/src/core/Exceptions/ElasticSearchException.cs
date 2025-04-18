namespace PaginationApp.Core.Exceptions
{
    public class ElasticsearchException : AppException
    {
        public ElasticsearchException(string message) : base(message, 503) { }
    }
}