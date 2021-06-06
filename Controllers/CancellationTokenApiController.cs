using CancellationTonkenCommand.Handler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CancellationTokenApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CancellationTokenApiController(IMediator mediator, ILogger<CancellationTokenApiController> logger)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("DependencyInjection")]
        public async Task<bool> GetDependencyInjectionAsync(CancellationToken token, string name, string description)
        {
            CategoryCommandRequest commandRequest = new(name, description, true);
            var retorno = await _mediator.Send(commandRequest, token);

            return retorno.Success;
        }
        [HttpPost]
        [Route("NewConnection")]
        public async Task<bool> GetNewConnectionAsync(CancellationToken token, string name, string description)
        {
            CategoryCommandRequest commandRequest = new(name, description, false);
            var retorno = await _mediator.Send(commandRequest, token);

            return retorno.Success;
        }
    }
}
