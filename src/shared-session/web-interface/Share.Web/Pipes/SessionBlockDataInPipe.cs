using System.Text;

namespace Pipes {
    public class SessionBlockDataInPipe : InPipe, ISessionBlockDataInPipe
    {
        private const string SessionBlockDataInPipeName = "/tmp/sessionInPipeForWebApiBlockData";
        private const string END_MESSAGE = "\n";
        private const string READ = "read ";
        private const string SAVE = "save ";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public SessionBlockDataInPipe() : base(SessionBlockDataInPipeName)
        {
        }

        public async Task SendReadAsync(string fileName) {
            await SendAsync(READ, fileName);
        }

        public async Task SendSaveAsync(string fileName) {
            await SendAsync(SAVE, fileName);
        }

        private async Task SendAsync(string command, string message)
        {
            var package = Encoding.UTF8.GetBytes(command + message + END_MESSAGE);

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