using Catalog.Service.EventHandlers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("v1/stocks")]
    public class ProductInStockController : ControllerBase
    {
        #region Variables
        /*private readonly IProductInStockQueryService _productInStockQueryService;*/
        private readonly ILogger<ProductInStockController> _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public ProductInStockController(ILogger<ProductInStockController> logger, /*IProductInStockQueryService productInStockQueryService, */IMediator mediator)
        {
            _logger = logger;
            //_productInStockQueryService = productInStockQueryService;
            _mediator = mediator;
        }
        #endregion

        [HttpPut]
        public async Task<IActionResult> Create(ProductInStockUpdateStockCommand command)
        {
            await _mediator.Publish(command);
            return NoContent();
        }
    }
}