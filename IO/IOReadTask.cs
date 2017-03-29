namespace FaceAuthService.IO
{
    class IOReadTask : IOTask
    {
        public string Path { get; private set; }

        public IOReadTask(string path) : base(IOTaskType.Read)
        {
            Path = path;
        }

        public void SignalCompletion(string data)
        {
            var result = new IOTaskResult{Success = data != null, Data = data};
            OnComplete(result);
        }
    }
}