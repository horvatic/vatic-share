namespace Pipes {
    public class PipeBuilder {

        // Command
        private const string SessionCommandInPipeName = "/tmp/sessionInPipeForWebApiCommandData";
        private const string WebApiCommandDataOutPipeName = "/tmp/webApiCommandDataOutPipeName";

        // File
        private const string SessionKeyDataInPipeName = "/tmp/sessionInPipeForWebApiKeyData";
        private const string WebApiKeyDataOutPipeName = "/tmp/webApiInPipeKeyData";
        private const string SessionBlockDataInPipeName = "/tmp/sessionInPipeForWebApiBlockData";
        private const string WebApiBlockDataOutPipeName = "/tmp/webApiInPipeBlockData";

        // Command
        public FileStream BuildSessionCommandInPipe() {
            return File.OpenWrite(SessionCommandInPipeName);
        }

        public StreamReader BuildWebApiCommandDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiCommandDataOutPipeName));
        }

        // File
        public FileStream BuildSessionBlockDataInPipe() {
            return File.OpenWrite(SessionBlockDataInPipeName);
        }

        public StreamReader BuildWebApiBlockDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiBlockDataOutPipeName));
        }

        public FileStream BuildSessionKeyDataInPipe() {
            return File.OpenWrite(SessionKeyDataInPipeName);
        }

        public StreamReader BuildWebApiKeyDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiKeyDataOutPipeName));
        }
    }
}