using Customer.Service.EventHandlers.Commands;
using Customer.Service.Queries.DTOs;
using Customer.Service.Queries.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Common.Collection;

namespace Customer.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/clients")]
    public class ClientController : ControllerBase
    {
        #region Variables
        private readonly IClientQueryService _clientQuerService;
        private readonly ILogger<ClientController> _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public ClientController(
            ILogger<ClientController> logger,
            IMediator mediator,
            IClientQueryService clientQuerService)
        {
            _logger = logger;
            _mediator = mediator;
            _clientQuerService = clientQuerService;
        }
        #endregion

        [HttpGet]
        public async Task<DataCollection<ClientDto>> GetAll(int page = 1, int take = 10, string ids = null)
        {
            IEnumerable<int> clients = null;

            if (!string.IsNullOrEmpty(ids))
            {
                clients = ids.Split(',').Select(x => Convert.ToInt32(x));
            }

            return await _clientQuerService.GetAllAsync(page, take, clients);
        }

        [HttpGet("{id}")]
        public async Task<ClientDto> Get(int id)
        {
            return await _clientQuerService.GetAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientCreateCommand notification)
        {
            await _mediator.Publish(notification);
            return Ok();
        }
    }
}
