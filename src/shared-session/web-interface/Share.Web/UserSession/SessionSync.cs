using System.Text;

namespace UserSession {
        public class SessionSync {
        private readonly List<UserSessionModel> _userSessions;
        private readonly FileStream _sessionInBlockDataPipe;
        private readonly StreamReader _apiOutKeyDataPipe;
        private readonly StreamReader _apiOutBlockDataPipe;

        private const string DATA_IN = "datain ";

        private const string READ = "read ";

        public SessionSync(FileStream sessionInBlockDataPipe, StreamReader apiOutKeyDataPipe, StreamReader apiOutBlockDataPipe) {
            _userSessions = new List<UserSessionModel>();
            _apiOutKeyDataPipe = apiOutKeyDataPipe;
            _apiOutBlockDataPipe = apiOutBlockDataPipe;
            _sessionInBlockDataPipe = sessionInBlockDataPipe;
        }

        public async Task SyncUserSession(UserSessionModel userSession) {
            var dataInEncoded = Encoding.UTF8.GetBytes(DATA_IN);
            var endMessage = Encoding.UTF8.GetBytes("\n");
            var spaceData = Encoding.UTF8.GetBytes(" ");
            var readEncoded = Encoding.UTF8.GetBytes(READ);
            var fileName = Encoding.UTF8.GetBytes("filename");

            await _sessionInBlockDataPipe.WriteAsync(readEncoded, 0, readEncoded.Length);
            await _sessionInBlockDataPipe.WriteAsync(fileName, 0, fileName.Length);
            await _sessionInBlockDataPipe.WriteAsync(endMessage, 0, endMessage.Length);

            var result = Encoding.UTF8.GetBytes(await _apiOutBlockDataPipe.ReadLineAsync() ?? "");

            await userSession.User.WriteRequest(result, result.Length);

            _userSessions.Add(userSession);
        }

        public async Task PushSessionData(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            var result = Encoding.UTF8.GetBytes(await _apiOutKeyDataPipe.ReadLineAsync() ?? "");
            foreach(var userSession in _userSessions) {
                if(userSession.User.IsOpen) {
                    await userSession.User.WriteRequest(result, result.Length);
                }
                else {
                    _userSessions.Remove(userSession);
                }
            }
        }
    }
}