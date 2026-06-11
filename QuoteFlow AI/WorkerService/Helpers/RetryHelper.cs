namespace WorkerService.Helpers
{
    public static class RetryHelper
    {
        public static async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxRetries, int delayMilliseconds)
        {
            int retries = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch
                {
                    retries++;
                    if (retries >= maxRetries) throw;
                    await Task.Delay(delayMilliseconds);
                }
            }
        }
    }
}