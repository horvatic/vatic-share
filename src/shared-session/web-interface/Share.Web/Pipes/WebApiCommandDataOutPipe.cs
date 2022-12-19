namespace Pipes {
    public class WebApiCommandDataOutPipe : OutPipe, IWebApiCommandDataOutPipe
    {
        private const string WebApiCommandDataOutPipeName = "/tmp/webApiInCommandData";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public WebApiCommandDataOutPipe() : base(WebApiCommandDataOutPipeName)
        {
        }

        public async Task<string?> ReadAsync()
        {
            await semaphore.WaitAsync();
            try {
                var result = await _stream.ReadLineAsync();
                semaphore.Release();
                return result;
            } 
            catch (Exception e) { 
                semaphore.Release();
                throw e;
            }
        }
    }
}