using System;
using System.Threading.Tasks;

namespace FaceAuthService.IO
{
    class IOTask
    {
        public IOTaskType Type { get; private set; }

        public Task<IOTaskResult> Task
        {
            get
            {
                return _tcs.Task;
            }
        }

        private TaskCompletionSource<IOTaskResult> _tcs;

        public IOTask(IOTaskType type)
        {
            Type = type;
            _tcs = new TaskCompletionSource<IOTaskResult>();
        }

        protected void OnComplete(IOTaskResult result)
        {
            _tcs.TrySetResult(result);
        }


    }

    enum IOTaskType
    {
        Write,
        Read
    }
}