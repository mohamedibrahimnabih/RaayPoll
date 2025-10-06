using FluentResults;

namespace RaayPoll.API.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(string message)
        : base(message)
        {
        }
    }
}
