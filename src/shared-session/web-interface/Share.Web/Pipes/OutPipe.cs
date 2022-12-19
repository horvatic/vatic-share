namespace Pipes {
    public abstract class OutPipe : IDisposable {
        protected readonly StreamReader _stream;

        public OutPipe(string pipeName) {
            _stream = new StreamReader(File.OpenRead(pipeName));
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}