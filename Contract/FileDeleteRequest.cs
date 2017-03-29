namespace FaceAuthService.Contract
{
    class FileDeleteRequest : Message
    {
        public string filename {get;set;}
        public FileDeleteRequest()
        {
            MessageType = MessageType.FileDeleteRequest;
        }
    }
}