using AutoMapper;
using LinkfireTechChallenge.Core.Commands.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;

        protected BaseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        protected virtual async Task<IActionResult> Retrieve<T>(IQueryModel queryModel, Func<Task<T>> func)
        {
            return Ok(await func());
        }

        protected virtual async Task<IActionResult> Retrieve<T, TDestination>(IQueryModel queryModel, Func<Task<T>> func)
            where T : class
            where TDestination : class
        {
            var result = await func();

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<TDestination>(result));

        }
    }
}
