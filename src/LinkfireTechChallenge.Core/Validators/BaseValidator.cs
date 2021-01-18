using FluentValidation;
using FluentValidation.Results;
using LinkfireTechChallenge.Core.Utils;

namespace LinkfireTechChallenge.Core.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        protected BaseValidator() { }

        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            Guard.NotNull(context, nameof(context));

            if (object.Equals(context.InstanceToValidate, default(T)))
            {
                Guard.NotNull(result, nameof(result));
                result.Errors.Add(new ValidationFailure(nameof(T), "Please ensure a model was supplied."));
                return false;
            }
            return true;
        }
    }
}
