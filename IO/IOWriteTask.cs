namespace FaceAuthService.IO
{
    class IOWriteTask : IOTask
    {
        public string Path { get; private set; }
        public string Data { get; private set; }

        public IOWriteTask(string path, string data) : base(IOTaskType.Write)
        {
            Path = path;
            Data = data;
        }

        public void SignalCompletion(bool succ)
        {
            var result = new IOTaskResult { Success = succ };
            OnComplete(result);
        }
    }
}