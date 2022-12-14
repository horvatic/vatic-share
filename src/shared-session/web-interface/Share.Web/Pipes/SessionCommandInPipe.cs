using System.Text;

namespace Pipes {
    public class SessionCommandInPipe : InPipe, ISessionCommandInPipe
    {
        private const string SessionInCommandData = "/tmp/sessionInCommandData";
        private const string CMD_OUT = "commanddata ";
        private const string END_MESSAGE = "\n";
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);
        public SessionCommandInPipe() : base(SessionInCommandData)
        {
        }

        public async Task SendAsync(string sessionId, string message)
        {
            var package = Encoding.UTF8.GetBytes(CMD_OUT + sessionId + " " + message + END_MESSAGE);

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