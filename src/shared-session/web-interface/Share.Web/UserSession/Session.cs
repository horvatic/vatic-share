using System.Text;

namespace UserSession {

    public class Session {
        private readonly User _user;
        private readonly FileStream _sessionInPipe;
        private readonly StreamReader _apiOutPipe;

        private readonly byte[] _sessionId;

        private const string DATA_IN = "datain ";
        private const string READ = "read ";

        public Session(User user, FileStream sessionInPipe, StreamReader apiOutPipe, string sessionId) {
            _user = user;
            _sessionInPipe = sessionInPipe;
            _apiOutPipe = apiOutPipe;
            _sessionId = Encoding.UTF8.GetBytes(sessionId);
        }

        public async Task Run() {
            var dataInEncoded = Encoding.UTF8.GetBytes(DATA_IN);
            var endMessage = Encoding.UTF8.GetBytes("\n");
            var spaceData = Encoding.UTF8.GetBytes(" ");
            var readEncoded = Encoding.UTF8.GetBytes(READ);

            var request = await _user.GetUserRequest();
            while (_user.IsOpen) {
                if(request.Array == null) {
                    continue ;
                }
                await _sessionInPipe.WriteAsync(dataInEncoded, 0, dataInEncoded.Length);
                await _sessionInPipe.WriteAsync(_sessionId, 0, _sessionId.Length);
                await _sessionInPipe.WriteAsync(spaceData, 0, spaceData.Length);
                await _sessionInPipe.WriteAsync(request.Array, 0, request.Count);
                await _sessionInPipe.WriteAsync(endMessage, 0, endMessage.Length);

                await _sessionInPipe.WriteAsync(readEncoded, 0, readEncoded.Length);
                await _sessionInPipe.WriteAsync(_sessionId, 0, _sessionId.Length);
                await _sessionInPipe.WriteAsync(endMessage, 0, endMessage.Length);
                
                var result = Encoding.UTF8.GetBytes(await _apiOutPipe.ReadLineAsync() ?? "No Data");

                await _user.WriteRequest(result, result.Length);

                request = await _user.GetUserRequest();
            }
            await _user.Close();
        }
    }
}