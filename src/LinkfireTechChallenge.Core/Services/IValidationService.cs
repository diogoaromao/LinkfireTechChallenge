using FluentValidation.Results;
using System.Collections.Generic;

namespace LinkfireTechChallenge.Core.Services
{
    public interface IValidationService
    {
        IEnumerable<string> Validate<T>(T entity);
        ValidationResult ValidateCommand(dynamic command);
    }
}
