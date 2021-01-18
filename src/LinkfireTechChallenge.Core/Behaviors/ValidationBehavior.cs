using LinkfireTechChallenge.Core.Commands.Core;
using LinkfireTechChallenge.Core.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TResponse : CommandResult
    {
        private readonly IValidationService _validationService;

        public ValidationBehavior(IValidationService validationService)
        {
            _validationService = validationService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationResult = _validationService.ValidateCommand(request);
            if (!validationResult.IsValid)
                return (TResponse)CommandResult.FromValidationResult(validationResult);

            return await next();
        }
    }
}
