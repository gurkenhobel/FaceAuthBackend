namespace FaceAuthService.Contract
{
    class FileDownloadResponseMessage : Message
    {
        public bool success { get; set; }
        public string data { get; set; }
        public FileDownloadResponseMessage()
        {
            MessageType = MessageType.FileDownloadResponse;
        }
    }
}