using System.Text;

namespace Pipes {
    public class SessionKeyDataInPipe : InPipe, ISessionKeyDataInPipe
    {
        private const string SessionKeyDataInFileData = "/tmp/sessionKeyDataInFileData";
        private const string END_MESSAGE = "\n";
        private const string DATA_IN = "datain ";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public SessionKeyDataInPipe() : base(SessionKeyDataInFileData)
        {
        }

        public async Task SendAsync(string openFile, string message)
        {
            var package = Encoding.UTF8.GetBytes(DATA_IN + openFile + " " + message + END_MESSAGE);

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