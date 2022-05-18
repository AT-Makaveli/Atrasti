namespace Atrasti.API.Models.Error
{
    public class ErrorEntry
    {
        public ErrorEntry(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; }
        public string Message { get; }
    }
}