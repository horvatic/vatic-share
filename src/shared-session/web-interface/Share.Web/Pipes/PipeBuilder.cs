namespace Pipes {
    public class PipeBuilder {

        // Command
        private const string SessionInCommandData = "/tmp/sessionInCommandData";
        private const string WebApiCommandDataOutPipeName = "/tmp/webApiInCommandData";

        // File
        private const string SessionKeyDataInFileData = "/tmp/sessionKeyDataInFileData";
         private const string WebApiKeyDataInFileData = "/tmp/webApiKeyDataInFileData";
        private const string SessionBlockDataInPipeName = "/tmp/sessionInPipeForWebApiBlockData";
        private const string WebApiBlockDataInFileData = "/tmp/webApiBlockDataInFileData";

        // Command
        public FileStream BuildSessionCommandInPipe() {
            return File.OpenWrite(SessionInCommandData);
        }

        public StreamReader BuildWebApiCommandDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiCommandDataOutPipeName));
        }

        // File
        public FileStream BuildSessionBlockDataInPipe() {
            return File.OpenWrite(SessionBlockDataInPipeName);
        }

        public StreamReader BuildWebApiBlockDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiBlockDataInFileData));
        }

        public FileStream BuildSessionKeyDataInPipe() {
            return File.OpenWrite(SessionKeyDataInFileData);
        }

        public StreamReader BuildWebApiKeyDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiKeyDataInFileData));
        }
    }
}