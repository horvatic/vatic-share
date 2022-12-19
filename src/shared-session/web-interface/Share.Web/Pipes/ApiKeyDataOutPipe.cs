namespace Pipes {
    public class ApiKeyDataOutPipe : OutPipe, IApiKeyDataOutPipe
    {
        private const string WebApiKeyDataInFileData = "/tmp/webApiKeyDataInFileData";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);
        public ApiKeyDataOutPipe() : base(WebApiKeyDataInFileData)
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