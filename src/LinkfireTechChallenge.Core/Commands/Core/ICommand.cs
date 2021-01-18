using MediatR;

namespace LinkfireTechChallenge.Core.Commands.Core
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
