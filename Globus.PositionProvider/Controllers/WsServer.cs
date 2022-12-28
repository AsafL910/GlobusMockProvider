using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using Globus.PositionProvider.Utils;
using System.Threading;

namespace Globus.PositionProvider.Controllers
{
    public static class WsServer 
    {
        public static WebSocketServer selfDataServer = new WebSocketServer("ws://0.0.0.0:7000");
        public static WebSocketServer mockDataServer = new WebSocketServer("ws://0.0.0.0:7001");

        public static void InitServer()
        {
             if (selfDataServer.WebSocketServices.Count == 0) {
                selfDataServer.AddWebSocketService<SelfData>("/real");
                selfDataServer.Start();
            }
            if (mockDataServer.WebSocketServices.Count == 0) {
                mockDataServer.AddWebSocketService<MockSelfData>("/mock");
                mockDataServer.Start();
            }
        }
    }

    public class MockSelfData : WebSocketBehavior
    {
    }

    public class SelfData : WebSocketBehavior
    {
        private static Aircraft aircraft = new Aircraft { CallSign = $"SelfData", Position = new Position { Latitude = Randomizer.RandomDouble(30,33), Longitude = Randomizer.RandomDouble(34.4,35.6) }, TrueTrack = Randomizer.RandomInt(0,360), Altitude = 0 };
        protected override void OnOpen() {
            aircraft.Simulate();

            while (true) 
            {
                Sessions.Broadcast(JsonConvert.SerializeObject(aircraft));
                Thread.Sleep(1000/60);
            }
        }
    }
}