namespace Pipes {
    public abstract class InPipe : IDisposable
    {
        protected readonly FileStream _stream;

        public InPipe(string pipeName) {
            _stream = File.OpenWrite(pipeName);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}