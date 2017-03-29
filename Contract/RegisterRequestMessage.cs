using System.Collections.Generic;
using FaceAuthService.Model;

namespace FaceAuthService.Contract
{
    class RegisterRequestMessage : Message
    {
        public List<string> Images {get;set;}
        public User User {get;set;}

        public RegisterRequestMessage()
        {
            MessageType = MessageType.RegisterRequest;
        }

    }
}