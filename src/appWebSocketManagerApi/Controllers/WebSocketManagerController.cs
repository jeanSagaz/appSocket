using appWebSocketManagerApi.Models;
using appWebSocketManagerApi.WebSocketHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace appWebSocketManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketManagerController : ControllerBase
    {
        private readonly ILogger<WebSocketManagerController> _logger;
        private readonly NotificationWebSocketHandler _notificationWebSocketHandler;

        public WebSocketManagerController(ILogger<WebSocketManagerController> logger,
            NotificationWebSocketHandler notificationWebSocketHandler)
        {
            _logger = logger;
            _notificationWebSocketHandler = notificationWebSocketHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string message)
        {
            var response = new WebSocketResponse
            {
                Id = Guid.NewGuid(),
                Value = message
            };
            await _notificationWebSocketHandler.SendMessageBroadcastAsync(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestBroadcastMessage model)
        {
            var response = new WebSocketResponse
            {
                Id = Guid.NewGuid(),
                Value = model.Message
            };
            await _notificationWebSocketHandler.SendMessageBroadcastAsync(response);

            return Ok(response);
        }

        [HttpPost("private")]
        public async Task<IActionResult> PrivateMessage([FromBody] RequestPrivateMessage model)
        {
            await _notificationWebSocketHandler.SendPrivateMessageAsync(model.SocketId, model.Message);

            return Ok(model);
        }
    }
}
