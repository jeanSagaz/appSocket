using appSignalRApi.Hubs.Interfaces;
using appSignalRApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace appSignalRApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignalRQueryController : ControllerBase
    {
        private readonly ILogger<SignalRController> _logger;
        private readonly IHubNotificationQueryHelper _hubNotificationQueryHelper;

        public SignalRQueryController(ILogger<SignalRController> logger,
            IHubNotificationQueryHelper hubNotificationHelper)
        {
            _logger = logger;
            _hubNotificationQueryHelper = hubNotificationHelper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_hubNotificationQueryHelper.GetOnlineHubs());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestBroadcastMessage model)
        {
            await _hubNotificationQueryHelper.SendNofificationToAll(model.Message);

            return Ok(model);
        }

        [HttpPost("private-group-message")]
        public async Task<IActionResult> PrivateGroupMessage([FromBody] RequestPrivateMessage model)
        {
            await _hubNotificationQueryHelper.SendPrivateGroupNofification(model.ConnectionId, model.Message);

            return Ok(model);
        }

        [HttpPost("private-message")]
        public async Task<IActionResult> PrivateMessage([FromBody] RequestPrivateMessage model)
        {
            await _hubNotificationQueryHelper.SendPrivateNofification(model.ConnectionId, model.Message);

            return Ok(model);
        }
    }
}
