using appSignalRApi.Hubs.Interface;
using appSignalRApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace appSignalRApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(_hubNotificationHelper.GetOnlineHubs());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestBroadcastMessage model)
        {
            await _hubNotificationHelper.SendNofificationToAll(model.Message);

            return Ok(model);
        }

        [HttpPost("private-message")]
        public async Task<IActionResult> PrivateMessage([FromBody] RequestPrivateMessage model)
        {
            await _hubNotificationHelper.SendPrivateNofification(model.ConnectionId, model.Message);

            return Ok(model);
        }
    }
}
