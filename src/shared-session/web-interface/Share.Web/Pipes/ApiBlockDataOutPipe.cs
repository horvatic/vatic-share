namespace Pipes {
    public class ApiBlockDataOutPipe : OutPipe, IApiBlockDataOutPipe
    {
        private const string WebApiBlockDataInFileData = "/tmp/webApiBlockDataInFileData";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public ApiBlockDataOutPipe() : base(WebApiBlockDataInFileData)
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