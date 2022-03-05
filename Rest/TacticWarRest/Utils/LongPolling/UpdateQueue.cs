namespace TacticWar.Rest.Utils.LongPolling
{
    public class UpdateQueue<T>
    {
        // Private fields
        private readonly object _locker = new();
        private readonly IList<TaskCompletionWrapper<T>> _taskSource = new List<TaskCompletionWrapper<T>> { new TaskCompletionWrapper<T>() };



        // Proeprties
        public bool IsWaitingForSnapshot { get; set; } = false;
        public int VersionFetched { get; private set; }



        // Public Methods
        public async Task<T> Get()
        {
            Task<T> retTask;
            lock (_locker)
            {
                var taskWrapper = _taskSource[0];
                taskWrapper.Used = true;
                retTask = taskWrapper.TaskCompletionSource.Task;
                CheckTaskCompleted();
                VersionFetched++;
            }
            var ret = await retTask;
            return ret;
        }

        public void NotifyNew(T t)
        {
            lock (_locker)
            {
                if (_taskSource.Last().Notified)
                    _taskSource.Add(new TaskCompletionWrapper<T>());
                var last = _taskSource.Last();
                last.Notified = true;
                last.TaskCompletionSource.SetResult(t);
                CheckTaskCompleted();
            }
        }



        // Utils
        private void CheckTaskCompleted()
        {
            var taskWrapper = _taskSource[0];
            if (taskWrapper.Notified && taskWrapper.Used)
            {
                _taskSource.RemoveAt(0);
                if (!_taskSource.Any())
                    _taskSource.Add(new TaskCompletionWrapper<T>());
            }
        }
    }

    public class TaskCompletionWrapper<T>
    {
        public TaskCompletionSource<T> TaskCompletionSource { get; } = new TaskCompletionSource<T>();
        public bool Used { get; set; }
        public bool Notified { get; set; }
    }

}
