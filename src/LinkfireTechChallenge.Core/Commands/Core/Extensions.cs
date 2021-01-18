using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace LinkfireTechChallenge.Core.Commands.Core
{
    public static class Extensions
    {
        public static IEnumerable<CommandError> ToCommandErrors(this IList<ValidationFailure> validationErrors)
        {
            return validationErrors.Select(e => new CommandError(CommandErrorType.InputError, e.ErrorMessage, e.PropertyName));
        }
    }
}
