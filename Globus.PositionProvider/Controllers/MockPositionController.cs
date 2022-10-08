using System.Threading.Tasks;
using Globus.PositionProvider.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using System.Threading;

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
        public async Task<ActionResult<long>> Post(Aircraft aircraft)
        {
            _logger.LogDebug("Post Request for MockData");
            MyTimer.StartTimer(aircraft);
            WsServer.mockDataServer.WebSocketServices.Broadcast(JsonConvert.SerializeObject(aircraft));

            while (MyTimer.isRunning){
                Thread.Sleep(1);
            }

            return Ok(MyTimer.stopwatch.ElapsedMilliseconds);
        }
    }


}
