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
            _sessionId = Encoding.ASCII.GetBytes(sessionId);
        }

        public async Task Run() {
            var dataInEncoded = Encoding.ASCII.GetBytes(DATA_IN);
            var endMessage = Encoding.ASCII.GetBytes("\n");
            var spaceData = Encoding.ASCII.GetBytes(" ");
            var readEncoded = Encoding.ASCII.GetBytes(READ);

            var request = await _user.GetUserRequest();
            while (_user.IsOpen) {
                if(request.Array != null) {
                    await _sessionInPipe.WriteAsync(dataInEncoded, 0, dataInEncoded.Length);
                    await _sessionInPipe.WriteAsync(_sessionId, 0, _sessionId.Length);
                    await _sessionInPipe.WriteAsync(spaceData, 0, spaceData.Length);
                    await _sessionInPipe.WriteAsync(request.Array, 0, request.Count);
                    await _sessionInPipe.WriteAsync(endMessage, 0, endMessage.Length);

                    await _sessionInPipe.WriteAsync(readEncoded, 0, readEncoded.Length);
                    await _sessionInPipe.WriteAsync(_sessionId, 0, _sessionId.Length);
                    await _sessionInPipe.WriteAsync(endMessage, 0, endMessage.Length);
                    
                    var result = Encoding.ASCII.GetBytes(await _apiOutPipe.ReadLineAsync() ?? "");

                    await _user.WriteRequest(result, result.Length);
                } else {
                    throw new Exception();
                }
                request = await _user.GetUserRequest();
            }
            await _user.Close();
        }
    }
}