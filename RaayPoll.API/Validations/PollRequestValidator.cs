using FluentValidation;

namespace RaayPoll.API.Validations
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(model => model.Name).MaximumLength(256);
        }
    }
}
