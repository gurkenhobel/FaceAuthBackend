namespace FaceAuthService.Contract
{
    class RegisterResponseMessage : Message
    {
        public bool success {get;set;}
        public RegisterResponseMessage()
        {
            MessageType = MessageType.RegisterResponse;
        }
    }
}