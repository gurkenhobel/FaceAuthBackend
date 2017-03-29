using FaceAuthService.Model;

namespace FaceAuthService.Contract
{
    class AuthResponseMessage : Message
    {
        public string name {get;set;}
        public bool success {get;set;}
        public AuthResponseMessage()
        {
            MessageType = MessageType.AuthResponse;
        }
    }
}