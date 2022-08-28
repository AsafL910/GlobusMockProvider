using System.Threading.Tasks;
using Globus.PositionProvider.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;

namespace Globus.PositionProvider.Controllers
{
    [ApiController]
    [Route("mockData")]
    public class MockPositionController : ControllerBase
    {
        private readonly ILogger<MockPositionController> _logger;

        private static WebSocketServer wsServer = new WebSocketServer("ws://0.0.0.0:7000");

        public MockPositionController(ILogger<MockPositionController> logger)
        {
            if (wsServer.WebSocketServices.Count == 0) {
                wsServer.AddWebSocketService<MockSelfData>("/mockData");
                wsServer.Start();
            }

            _logger = logger;

            _logger.LogDebug("MockData Created");
        }

        [HttpPost]
        public async Task<ActionResult<long>> Post(Aircraft aircraft)
        {
            _logger.LogDebug("Post Request for MockData");
            MyTimer.StartTimer(aircraft);
            wsServer.WebSocketServices.Broadcast(JsonConvert.SerializeObject(aircraft));

            while (MyTimer.isRunning){
                System.Threading.Thread.Sleep(1);
            }

            return Ok(MyTimer.stopwatch.ElapsedMilliseconds);
        }
    }

    public class MockSelfData : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs eventArgs) {
            var parsedData = JsonConvert.DeserializeObject<Aircraft>(eventArgs.Data);
            MyTimer.StopTimer(parsedData);
        }
    }
}
