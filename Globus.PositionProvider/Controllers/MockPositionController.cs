using System.Threading.Tasks;
using Globus.PositionProvider.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;

namespace Globus.PositionProvider.Controllers
{
    [ApiController]
    [Route("self-data")]
    public class MockPositionController : ControllerBase
    {
        private readonly ILogger<MockPositionController> _logger;

        public MockPositionController(ILogger<MockPositionController> logger)
        {

            _logger = logger;

            _logger.LogDebug("MockData Created");
        }

        [HttpPost]
        public async Task<ActionResult<Aircraft>> Post(Aircraft aircraft)
        {
            WsServer.mockDataServer.WebSocketServices.Broadcast(JsonConvert.SerializeObject(aircraft));
            return Ok(aircraft);
        }
    }


}
