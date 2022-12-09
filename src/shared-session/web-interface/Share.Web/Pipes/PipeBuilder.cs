namespace Pipes {
    public class PipeBuilder {

        private const string SessionInPipeName = "/tmp/sessionInPipeForWebApi";
        private const string WebApiOutPipeName = "/tmp/webApiInPipe";

        public FileStream BuildSessionInPipe() {
            return File.OpenWrite(SessionInPipeName);
        }

        public StreamReader BuildWebApiOutPipe() {
            return new StreamReader(File.OpenRead(WebApiOutPipeName));
        }
    }
}