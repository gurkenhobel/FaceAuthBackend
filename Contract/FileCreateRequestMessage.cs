namespace FaceAuthService.Contract
{
    class FileCreateRequestMessage : Message
    {
        public string name { get; set; }
        public bool encrypted { get; set; }
        public string data {get;set;}
        public FileCreateRequestMessage()
        {
            MessageType = MessageType.FileCreateRequest;
        }
    }

}