using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace FaceAuthService
{
    public class Program
    {
        private static WebSocketServer _server;

        public static void Main(string[] args)
        {

            _server = new WebSocketServer(Config.LoadPort());
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
                    case "connect_api":
                        FaceAPIConnection.Instance.Connect();
                        break;
                    case "db_add":
                        Database.Instance.InsertUser();
                        break;
                    case "db_get":
                        Database.Instance.GetAllUsers();
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
