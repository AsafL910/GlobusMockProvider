using System.Diagnostics;
using System;

namespace Globus.PositionProvider.Utils 
{
    public static class MyTimer 
    {
        public static Aircraft mockAircraft;
        public static bool isRunning = false;
        public static Stopwatch stopwatch = new Stopwatch();

        public static void StartTimer(Aircraft aircraft)
        {
            mockAircraft = aircraft;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            isRunning = true;
        }

        public static void StopTimer(Aircraft aircraft) {
            Console.WriteLine($"expected: {mockAircraft.CallSign}, recieved: {aircraft.CallSign}");

            if (mockAircraft.CallSign.Equals(aircraft.CallSign)) {
                stopwatch.Stop();
                isRunning = false;
                Console.WriteLine($"Elapsed Time: {stopwatch.ElapsedMilliseconds}");
            }
        }
    }
}