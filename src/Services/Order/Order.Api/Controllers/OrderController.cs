using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Service.EventHandlers.Commands;
using Order.Service.Queries.DTOs;
using Order.Service.Queries.Interfaces;
using Service.Common.Collection;

namespace Order.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/orders")]
    public class OrderController : ControllerBase
    {
        #region Variables
        private readonly IOrderQueryService _orderQueryService;
        private readonly ILogger<OrderController> _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public OrderController(
            ILogger<OrderController> logger,
            IMediator mediator,
            IOrderQueryService orderQueryService)
        {
            _logger = logger;
            _mediator = mediator;
            _orderQueryService = orderQueryService;
        }
        #endregion

        [HttpGet]
        public async Task<DataCollection<OrderDto>> GetAll(int page = 1, int take = 10)
        {
            return await _orderQueryService.GetAllAsync(page, take);
        }

        [HttpGet("{id}")]
        public async Task<OrderDto> Get(int id)
        {
            return await _orderQueryService.GetAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateCommand notification)
        {
            await _mediator.Publish(notification);
            return Ok();
        }
    }
}
