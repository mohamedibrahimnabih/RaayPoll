using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace RaayPoll.API.Validations
{
    public class NotPastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.UtcNow.Date)
                {
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} cannot be in the past.";
        }
    }
}
