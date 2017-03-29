namespace FaceAuthService.Contract
{
    class AuthRequestMessage : Message
    {
        public string Image {get; set;}

        
        public AuthRequestMessage()
        {
            MessageType = MessageType.AuthRequest;

        }
    }
}