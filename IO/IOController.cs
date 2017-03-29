using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.IO;
using System;

namespace FaceAuthService.IO
{
    class IOController
    {
        public static IOController Instance { get; private set; } = new IOController();

        private bool _isWorking;
        private ObservableConcurrentQueque<IOTask> WorkQueque;
        protected IOController()
        {
            WorkQueque = new ObservableConcurrentQueque<IOTask>();
            WorkQueque.CollectionChanged += (s, e) =>
            {
                if (!_isWorking)
                    DoWork();
            };

            var test = new ConcurrentQueue<IOTask>();
        }

        private async Task DoWork()
        {
            _isWorking = true;
            await Task.Run(() =>
           {
               while (WorkQueque.Count > 0)
               {
                   var task = WorkQueque.Dequeque();
                   if (task != null)
                   {
                       switch (task.Type)
                       {
                           case IOTaskType.Read:
                               var tr = (IOReadTask)task;
                               var data = ReadFile(tr.Path);
                               tr.SignalCompletion(data);
                               break;
                           case IOTaskType.Write:
                               var tw = (IOWriteTask)task;
                               var succ = WriteFile(tw.Path, tw.Data);
                               tw.SignalCompletion(succ);
                               break;
                           case IOTaskType.Delete:
                                var td = (IODeleteTask)task;
                                td.SignalCompletion(DeleteFile(td.Path));
                                break;
                       }
                   }
               }
           });
            _isWorking = false;
        }

        private string ReadFile(string path)
        {
            try
            {
            var res = "";
            res = File.ReadAllText(path + ".dat");
            return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ReadFile: {ex.Message}");
                return "";
            }
        }

        private bool WriteFile(string path, string data)
        {
            try
            {
                File.WriteAllText(path + ".dat", data);
                return true;
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"WriteFile: {ex.Message}");
                return false;
            }
        }

        private bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteFile: {ex.Message}");
                return false;
            }
        }

        public async Task Write(string path, string data)
        {
            var task = new IOWriteTask(path, data);
            WorkQueque.Enqueque(task);
            await task.Task;
        }

        public async Task<string> Read(string path)
        {
            var task = new IOReadTask(path);
            WorkQueque.Enqueque(task);
            var res = await task.Task;
            return res.Data;
        }

        public async Task Delete(string path)
        {
            var task = new IODeleteTask(path);
            WorkQueque.Enqueque(task);
            await task.Task;
        }

    }
}