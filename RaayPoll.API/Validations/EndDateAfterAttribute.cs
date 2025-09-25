using System.ComponentModel.DataAnnotations;

namespace RaayPoll.API.Validations
{
    public class EndDateAfterAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public EndDateAfterAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var endDate = value as DateTime?;
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var startDate = startDateProperty?.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (endDate.HasValue && startDate.HasValue && endDate <= startDate)
            {
                return new ValidationResult("End date must be later than the start date.");
            }

            return ValidationResult.Success;
        }
    }
}
