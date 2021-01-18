using FluentValidation.Results;
using LinkfireTechChallenge.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LinkfireTechChallenge.Core.Commands.Core
{
    public class CommandResult : ObjectResult
    {
        public bool IsSuccessful { get; }
        public IEnumerable<ICommandError> Errors { get; }
        public string ErrorsMessage => string.Join(", ", Errors?.Select(e => e.Message));
        public Exception Exception { get; set; }

        public CommandResult(HttpStatusCode httpStatusCode) : this(httpStatusCode, null, null)
        {
        }

        public CommandResult(HttpStatusCode httpStatusCode, object content) : this(httpStatusCode, null, content)
        {
        }

        public CommandResult(HttpStatusCode httpStatusCode, IEnumerable<ICommandError> errors) : this(httpStatusCode, errors, null)
        {
        }

        public CommandResult(HttpStatusCode httpStatusCode, IEnumerable<ICommandError> errors, object content) : base(content)
        {
            IsSuccessful = httpStatusCode < HttpStatusCode.BadRequest;
            StatusCode = (int)httpStatusCode;
            Errors = errors ?? Enumerable.Empty<ICommandError>();
        }


        public static CommandResult NoContent()
        {
            return new CommandResult(HttpStatusCode.NoContent);
        }

        public static CommandResult Ok()
        {
            return new CommandResult(HttpStatusCode.OK);
        }

        public static CommandResult Ok(object content)
        {
            return new CommandResult(HttpStatusCode.OK, content);
        }

        public static CommandResult PreConditionFailed()
        {
            return new CommandResult(HttpStatusCode.PreconditionFailed);
        }

        public static CommandResult FromException(Exception ex)
        {
            return Fail(HttpStatusCode.BadRequest, new CommandError(CommandErrorType.Exception, ex, null), ex);
        }

        public static CommandResult BadRequest(string message)
        {
            return Fail(HttpStatusCode.BadRequest, new CommandError(CommandErrorType.DefaultError, message, null));
        }

        public static CommandResult NotFound(string name, string value)
        {
            return Fail(HttpStatusCode.NotFound, new CommandError(CommandErrorType.NotFound, "Error" /*string.Format(CultureInfo.InvariantCulture, NotificationResources.NotFound, name, value)*/, name));
        }

        public static CommandResult Fail(HttpStatusCode httpStatusCode, ICommandError error)
        {
            return Fail(httpStatusCode, error, null);
        }

        public static CommandResult Fail(HttpStatusCode httpStatusCode, ICommandError error, Exception exception)
        {
            var commandResult = new CommandResult(httpStatusCode, new[] { error }, error)
            {
                Exception = exception
            };
            return commandResult;
        }

        public static CommandResult FromValidationResult(ValidationResult validationResult)
        {
            return FromValidationResult(validationResult, null);
        }
        public static CommandResult FromValidationResult(ValidationResult validationResult, Exception exception)
        {
            Guard.NotNull(validationResult, nameof(validationResult));

            if (validationResult.IsValid)
                return NoContent();

            var errors = validationResult.Errors.ToCommandErrors();
            return new CommandResult(HttpStatusCode.BadRequest, errors, errors)
            {
                Exception = exception
            };
        }
    }
}
