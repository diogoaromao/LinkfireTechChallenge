using FluentValidation;
using FluentValidation.Results;
using LinkfireTechChallenge.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LinkfireTechChallenge.Core.Services
{
    public class FluentValidationService : IValidationService
    {
        private readonly IValidatorFactory _validatorFactory;

        public FluentValidationService(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public IEnumerable<string> Validate<T>(T entity)
        {
            var validator = _validatorFactory.GetValidator<T>();
            var result = validator.Validate(entity);

            return result.Errors?.Select(e => e.ErrorMessage);
        }

        public ValidationResult ValidateCommand(dynamic command)
        {
            Guard.NotNull(command, nameof(command));

            var type = command.GetType();
            var validator = _validatorFactory.GetValidator(type);
            if (validator == null)
                throw new ValidationException($"Validator for type {nameof(type)} could not be resolved.");

            return validator.Validate(command);
        }
    }
}
