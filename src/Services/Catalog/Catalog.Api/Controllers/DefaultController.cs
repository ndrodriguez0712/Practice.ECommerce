using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
        #region Variables
        private readonly ILogger<DefaultController> _logger;
        #endregion

        #region Constructor
        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }
        #endregion

        [HttpGet]
        public string Get()
        {
            return "Running...";
        }
    }
}
