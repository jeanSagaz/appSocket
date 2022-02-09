using appSignalRApi.Hubs.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace appSignalRApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignalRController : ControllerBase
    {
        private readonly ILogger<SignalRController> _logger;
        private readonly IHubNotificationHelper _hubNotificationHelper;        

        public SignalRController(ILogger<SignalRController> logger,
            IHubNotificationHelper hubNotificationHelper)
        {
            _logger = logger;
            _hubNotificationHelper = hubNotificationHelper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_hubNotificationHelper.GetOnlineUsers());
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok(_hubNotificationHelper.GetOnlineUsers());
        }
    }
}
