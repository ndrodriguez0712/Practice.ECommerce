using Identity.Service.Queries.DTOs;
using Identity.Service.Queries.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Common.Collection;

namespace Identity.Api.Controllers
{
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly IUserQueryService _userQueryService;
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public UserController(ILogger<UserController> logger, IMediator mediator, IUserQueryService userQueryService)
        {
            _logger = logger;
            _mediator = mediator;
            _userQueryService = userQueryService;
        }
        #endregion

        [HttpGet]
        public async Task<DataCollection<UserDto>> GetAll(int page = 1, int take = 10)
        {            
            return await _userQueryService.GetAllAsync(page, take);
        }

        [HttpGet("{id}")]
        public async Task<UserDto> Get(int id)
        {
            return await _userQueryService.GetAsync(id);
        }
    }
}
