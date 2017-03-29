using System;
using System.Collections.Generic;
using System.Text;

namespace FaceAuthService.IO
{
    class IODeleteTask : IOTask
    {
        public string Path { get; private set; }

        public IODeleteTask(string path) : base(IOTaskType.Delete)
        {
            Path = path;
        }

        public void SignalCompletion(bool success)
        {
            var res = new IOTaskResult { Success = success, Data = null };
            OnComplete(res);
        }
    }
}
