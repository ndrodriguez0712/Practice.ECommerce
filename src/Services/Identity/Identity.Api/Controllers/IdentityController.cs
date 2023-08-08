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

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create(UserCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _mediator.Send(command);

                    if (!result.Succeeded)
                    {
                        return StatusCode(500, result.Errors);
                    }

                    return Ok();
                }
                catch (Exception ex)
                {
                    //ErrorLogService.SaveErrorLog(0, Assembly.GetEntryAssembly().GetName().Name, LogError.GetErrorDescription(EnumErrorCode.OMS9999.ToString()), EnumErrorCode.OMS9999, ex, "", 0, 0);
                    return StatusCode(500, ex);
                }                
            }

            return BadRequest();
        }

        [HttpPost("authentication")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IdentityAccess))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Authentication(UserLoginCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _mediator.Send(command);

                    if (!result.Succeeded)
                    {
                        return Unauthorized("Access denied");
                    }

                    return Ok(result);
                }
                catch(Exception ex) 
                {
                    //ErrorLogService.SaveErrorLog(0, Assembly.GetEntryAssembly().GetName().Name, LogError.GetErrorDescription(EnumErrorCode.OMS9999.ToString()), EnumErrorCode.OMS9999, ex, "", 0, 0);
                    return Unauthorized(ex);
                }
            }

            return BadRequest();
        }
    }
}
