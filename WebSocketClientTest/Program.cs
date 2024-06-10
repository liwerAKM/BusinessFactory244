using System;
using System.Threading;

namespace WebSocketClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSocketClient webSocketClient = new WebSocketClient();
            webSocketClient.Start();

            bool goon = true;
            while (goon)
            {
                string input = Console.ReadLine();
                if (input.ToUpper() == "Q")
                {

                    goon = false;
                }
                else if (input.ToUpper() == "T1")
                {
                    webSocketClient.CallTest();
                }
                else if (input.ToUpper() == "T3")
                {
                    webSocketClient.Call2001();
                }
                else if (input.ToUpper() == "T4")
                {
                    webSocketClient.CallT4();
                }
            }
            Exit(5);
        }

        internal static void Exit(int SleepSecond)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Procedure will stop in 5 second... \r\n");
            Thread.Sleep(1000 * SleepSecond);
            Environment.Exit(0);

        }
    }
 
}
