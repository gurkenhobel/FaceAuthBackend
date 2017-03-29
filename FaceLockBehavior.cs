using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using Microsoft.ProjectOxford.Face;
using Newtonsoft.Json;
using FaceAuthService.Contract;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using FaceAuthService.Model;
using FaceAuthService.IO;

namespace FaceAuthService
{

    public class FaceLockBehavior : WebSocketBehavior
    {

        private User _user;

        private string UserPath
        {
            get
            {
                return _user == null ? null : Directory.GetCurrentDirectory() + "/data/" + _user._id.ToString();
            }
        }

        public FaceLockBehavior()
        {

        }



        protected override void OnClose(CloseEventArgs e)
        {
            var name = _user == null ? "unknown user" : _user.name;
            if (e.Code == 4200)
                Console.WriteLine($"{name} disconnected");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = JsonConvert.DeserializeObject<Message>(e.Data);
            var msg_type = msg.MessageType;
            Console.WriteLine(msg.MessageType);
            switch (msg_type)
            {
                case MessageType.AuthRequest:
                    HandleAuthRequest(JsonConvert.DeserializeObject<AuthRequestMessage>(e.Data));
                    break;
                case MessageType.RegisterRequest:
                    HandleRegisterRequest(JsonConvert.DeserializeObject<RegisterRequestMessage>(e.Data));
                    break;
                case MessageType.FileCatalogRequest:
                    HandleFileCatalogRequest();
                    break;
                case MessageType.FileCreateRequest:
                    HandleFileCreateRequest(JsonConvert.DeserializeObject<FileCreateRequestMessage>(e.Data));
                    break;
                case MessageType.FileDeleteRequest:
                    HandleFileDeleteRequest(JsonConvert.DeserializeObject<FileDeleteRequest>(e.Data));
                    break;
                case MessageType.FileDownloadRequest:
                    HandleFileDownloadRequest(JsonConvert.DeserializeObject<FileDownloadRequestMessage>(e.Data));
                    break;
            }
        }

        private async Task HandleFileCatalogRequest()
        {
            var success = false;
            try
            {
                await SendFileCatalogUpdate(_user.locker);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task HandleRegisterRequest(RegisterRequestMessage msg)
        {
            var success = false;
            try
            {
                var imgData = msg.Images.Select(i => Convert.FromBase64String(i));


                var newUser = await Database.Instance.AddUser(msg.User);

                Console.WriteLine($"RegisterRequest: images to send: {imgData.Count()} \n user: {newUser.ToJson()}");

                if (await FaceAPIConnection.Instance.RegisterUser(newUser, imgData))
                {
                    Console.WriteLine($"new User registered: {newUser._id}");
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HandleRegister: {ex.Message}");
                success = false;
            }
            await SendRegisterResponse(success);
        }

        private async Task HandleAuthRequest(AuthRequestMessage msg)
        {
            try
            {
                var imgData = Convert.FromBase64String(msg.Image);

                var id = await FaceAPIConnection.Instance.Identify(imgData);
                Console.WriteLine($"HandleAuth.UserId: {id.ToString()}");

                _user = Database.Instance.GetUser(id);

                if (!Directory.Exists(UserPath))
                    Directory.CreateDirectory(UserPath);

                await SendAuthResponse(_user);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"HandleAuth: {ex.Message}");
                Console.WriteLine(UserPath);
                await SendAuthResponse(null);
            }
        }
        private async Task HandleFileCreateRequest(FileCreateRequestMessage msg)
        {
            var file = new LockerObject { name = msg.name, encrypted = msg.encrypted };
            Database.Instance.AddFile(_user, file);
            await IOController.Instance.Write($"{UserPath}/{msg.name}", msg.data);
            _user = Database.Instance.GetUser(_user._id);
            await SendFileCatalogUpdate(_user.locker);
        }

        private async Task HandleFileDeleteRequest(FileDeleteRequest msg)
        {
            var fileInfo = new LockerObject { name = msg.filename };
            Database.Instance.DeleteFile(_user, fileInfo);
            IOController.Instance.Delete($"{UserPath}/{msg.filename}");
            _user = Database.Instance.GetUser(_user._id);
            await SendFileCatalogUpdate(_user.locker);
        }

        private async Task HandleFileDownloadRequest(FileDownloadRequestMessage msg)
        {
            var filepath = $"{UserPath}/{msg.filename}";
            string data = await IOController.Instance.Read(filepath);
            var resp = new FileDownloadResponseMessage { success = data != null, data = data };
            Console.WriteLine($"file length: {data.Length}");
            await SendMessage(resp);
        }

        private async Task SendFileCatalogUpdate(LockerDirectory fileCatalog)
        {
            var msg = new FileCatalogUpdateMessage { files = fileCatalog.content };
            await SendMessage(msg);
        }

        private async Task SendAuthResponse(User user)
        {
            var msg = new AuthResponseMessage { name = user?.name, success = user != null };
            await SendMessage(msg);
        }
        private async Task SendRegisterResponse(bool success)
        {
            var msg = new RegisterResponseMessage { success = success };
            await SendMessage(msg);
        }

        private async Task SendMessage(Message msg)
        {
            var json = JsonConvert.SerializeObject(msg);
            await Task.Run(() => Send(json));
        }
    }
}