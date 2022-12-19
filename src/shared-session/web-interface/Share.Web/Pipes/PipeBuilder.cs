namespace Pipes {
    public class PipeBuilder {

        private const string WebApiCommandDataOutPipeName = "/tmp/webApiInCommandData";

        public StreamReader BuildWebApiCommandDataOutPipe() {
            return new StreamReader(File.OpenRead(WebApiCommandDataOutPipeName));
        }

    }
}