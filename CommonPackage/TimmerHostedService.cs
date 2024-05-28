using Microsoft.Extensions.Hosting;

namespace CommonPackage
{

    /// <summary>
    /// The Timer doesn't wait for previous executions of DoWork to finish, so the approach shown might 
    /// not be suitable for every scenario. Interlocked.Increment is used to increment the execution counter
    /// as an atomic operation, which ensures that multiple threads don't update executionCount concurrently.
    /// </summary>
    public abstract class TimmerHostedService : IHostedService
    {
        private const int timeOutSecond = 5;

        private int executionCount = 0;

        private Timer? _timer = null;

        public TimmerHostedService()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(timeOutSecond));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public virtual void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);


        }
    }
}
