namespace FaceAuthService.Contract
{
    class FileDownloadRequestMessage :Message
    {
        public string filename {get; set;}
        public FileDownloadRequestMessage()
        {
            MessageType = MessageType.FileDownloadRequest;
        }
    }
}