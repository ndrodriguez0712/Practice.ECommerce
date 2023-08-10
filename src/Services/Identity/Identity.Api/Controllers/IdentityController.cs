using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("v1/identity")]
    public class IdentityController : ControllerBase
    {
        #region Variables
        private readonly ILogger<IdentityController> _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public IdentityController(
            ILogger<IdentityController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        #endregion

        [HttpPost("Create")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create(UserCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                {
                    return StatusCode(500, result.Errors);
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IdentityAccess))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Authentication(UserLoginCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                {
                    return Unauthorized("Access denied");
                }

                return Ok(result);
            }

            return BadRequest();
        }
    }
}
