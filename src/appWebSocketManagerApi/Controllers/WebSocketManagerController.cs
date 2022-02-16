using appWebSocketManagerApi.Models;
using appWebSocketManagerApi.WebSocketHandlers;
using appWebSocketManagerApi.WebSocketHandlers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace appWebSocketManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketManagerController : ControllerBase
    {
        private readonly ILogger<WebSocketManagerController> _logger;
        private readonly IWebSocketManagement _webSocketManagement;
        private readonly NotificationWebSocketHandler _notificationWebSocketHandler;

        public WebSocketManagerController(ILogger<WebSocketManagerController> logger,
            IWebSocketManagement webSocketManagement,
            NotificationWebSocketHandler notificationWebSocketHandler)
        {
            _logger = logger;
            _webSocketManagement = webSocketManagement;
            _notificationWebSocketHandler = notificationWebSocketHandler;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_webSocketManagement.OnlineWebSockets());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestBroadcastMessage model)
        {
            var webSocketMessage = new WebSocketMessage
            {
                Id = Guid.NewGuid(),
                Value = model.Message
            };
            await _notificationWebSocketHandler.SendMessageBroadcastAsync(webSocketMessage);

            return Ok(webSocketMessage);
        }

        [HttpPost("private-message")]
        public async Task<IActionResult> PrivateMessage([FromBody] RequestPrivateMessage model)
        {
            await _notificationWebSocketHandler.SendPrivateMessageAsync(model.SocketId, model.Message);

            return Ok(model);
        }
    }
}
