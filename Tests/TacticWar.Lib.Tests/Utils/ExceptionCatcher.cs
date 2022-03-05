using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TacticWar.Test.TacticWar.Lib.Tests.Utils
{
    public class ExceptionCatcher : IAsyncDisposable
    {
        // Events
        public event Action<Exception>? OnException;



        // Private fields
        private List<Task> _tasks = new ();



        // Core
        public Action DoAsync(Func<Task> asyncAction) => () => _tasks.Add(TryDo(asyncAction));


        public Action<T> Action<T>(Func<T, Task> asyncFunc) => x => DoAsync(async () => await asyncFunc.Invoke(x)).Invoke();
        public async Task ThrowIfAnyException() => await Task.WhenAll(_tasks);


        private async Task TryDo(Func<Task> asyncAction)
        {
            try
            {
                await asyncAction.Should().NotThrowAsync();
            }
            catch (Exception e)
            {
                OnException?.Invoke(e);
                throw;
            }
        }



        // IDisposable
        public async ValueTask DisposeAsync() => await ThrowIfAnyException();
    }
}
