namespace Pipes {
    public class PipeBuilder {

        // Command
        private const string SessionInCommandData = "/tmp/sessionInCommandData";
        private const string WebApiCommandDataOutPipeName = "/tmp/webApiInCommandData";

         private const string WebApiKeyDataInFileData = "/tmp/webApiKeyDataInFileData";

        // Command
        public FileStream BuildSessionCommandInPipe() {
            return File.OpenWrite(SessionInCommandData);
        }

        public StreamReader BuildWebApiCommandDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiCommandDataOutPipeName));
        }

        public StreamReader BuildWebApiKeyDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiKeyDataInFileData));
        }
    }
}