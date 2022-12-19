using System.Text;

namespace Pipes {
    public class SessionKeyDataInPipe : InPipe, ISessionKeyDataInPipe
    {
        private const string SessionKeyDataInFileData = "/tmp/sessionKeyDataInFileData";
        private const string END_MESSAGE = "\n";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public SessionKeyDataInPipe() : base(SessionKeyDataInFileData)
        {
        }

        public async Task SendAsync(string message)
        {
            var package = Encoding.UTF8.GetBytes(message + END_MESSAGE);

            await semaphore.WaitAsync();
            try {
                await _stream.WriteAsync(package, 0, package.Length);
                semaphore.Release();
            } 
            catch (Exception e) { 
                semaphore.Release();
                throw e;
            }
        }
    }
}