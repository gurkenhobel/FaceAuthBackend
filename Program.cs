using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace FaceAuthService
{
    public class Program
    {
        private static WebSocketServer _server;

        public static void Main(string[] args)
        {
            var cfg = Configuration.LoadConfig();
            _server = new WebSocketServer(int.Parse(cfg["port"]));
            _server.AddWebSocketService<FaceLockBehavior>("/facelock");
            FaceAPIConnection.Instance.Connect();

            _server.Start();

            var running = true;
            do
            {
                switch (Console.ReadLine())
                {
                    case "exit":
                        running = false;
                        break;
                    case "group_create":
                        FaceAPIConnection.Instance.CreateGroup();
                        break;
                }
            }
            while (running);

            _server.Stop();
            Task.Delay(1000).Wait();
        }

       
    }
}
