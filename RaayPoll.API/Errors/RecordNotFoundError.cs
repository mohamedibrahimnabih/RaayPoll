using FluentResults;

namespace RaayPoll.API.Errors
{
    public class RecordNotFoundError : Error
    {
        public RecordNotFoundError(string message)
        : base(message)
        {
        }
    }
}
