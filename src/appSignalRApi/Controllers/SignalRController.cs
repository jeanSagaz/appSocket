using appSignalRApi.Hubs.Interface;
using appSignalRApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Get([FromQuery] string message)
        {
            await _hubNotificationHelper.SendNofificationToAll(message);

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestBroadcastMessage model)
        {
            await _hubNotificationHelper.SendNofificationToAll(model.Message);

            return Ok(model.Message);
        }
    }
}
