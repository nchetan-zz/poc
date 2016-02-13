using System;
using System.Threading;

namespace RoomHeater
{
    public class Program
    {
        private static readonly TimeSpan WaitTimespan = new TimeSpan(0, 0, 0, 0, 10);
        public static void Main(string[] arguments)
        {
            var threadCount = 1;

            if (arguments != null && arguments.Length > 0)
                threadCount = Int32.Parse(arguments[0]);

            var stopThreadEvents = new AutoResetEvent[threadCount];
            for (var threadIndex = 0; threadIndex < threadCount; ++threadIndex)
            {
                stopThreadEvents[threadIndex] = new AutoResetEvent(false);
            }

            var response = string.Empty;
            while (response != "Y")
            {
                var readLine = Console.ReadLine();
                if (readLine != null) response = readLine.ToUpperInvariant();
            }

            for (var threadIndex = 0; threadIndex < threadCount; ++threadIndex)
            {
                var resetEvent = stopThreadEvents[threadIndex];
                resetEvent.Set();
            }
        }

        public static void ThreadProc(Object threadEvent)
        {
            var resetEvent = (AutoResetEvent) threadEvent;
            uint value = 0;
            while (true)
            {
                value++;   
                if (resetEvent.WaitOne(WaitTimespan)) break;
            }
        }
    }
}
