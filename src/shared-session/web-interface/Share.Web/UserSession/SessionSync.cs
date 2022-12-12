using System.Text;
using MessageBus;

namespace UserSession {
    public class SessionSync {
        private readonly List<UserSessionModel> _userSessions;
        private readonly FileStream _sessionInBlockDataPipe;
        private readonly StreamReader _apiOutKeyDataPipe;
        private readonly StreamReader _apiOutBlockDataPipe;

        private readonly Message _message;

        private const string DATA_IN = "datain ";

        private const string READ = "read ";
        private const string FILE_DATA_OUT = "filedata ";

        private const string MESSAGE_OUT = "message ";

        public SessionSync(FileStream sessionInBlockDataPipe, StreamReader apiOutKeyDataPipe, StreamReader apiOutBlockDataPipe, Message message) {
            _userSessions = new List<UserSessionModel>();
            _apiOutKeyDataPipe = apiOutKeyDataPipe;
            _apiOutBlockDataPipe = apiOutBlockDataPipe;
            _sessionInBlockDataPipe = sessionInBlockDataPipe;
            _message = message;
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

            var rawFilePackage = await _apiOutBlockDataPipe.ReadLineAsync() ?? "";
            var filePackage = "";
            if(rawFilePackage != "") {
                filePackage = Encoding.UTF8.GetString(Convert.FromBase64String(rawFilePackage));
            }

            await userSession.User.WriteRequest(filePackage, FILE_DATA_OUT);

            _userSessions.Add(userSession);
        }

        public async Task PushSessionData(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            var rawFilePackage = await _apiOutKeyDataPipe.ReadLineAsync() ?? ""; // CAUSES SYNC
            var filePackage = "";
            if(rawFilePackage != "") {
                filePackage = Encoding.UTF8.GetString(Convert.FromBase64String(rawFilePackage));
            }
            var messages = new List<string>();
            while(_message.HasMessage()) {
                messages.Add(_message.Fetch());
            }
            foreach(var userSession in _userSessions) {
                if(userSession.User.IsOpen) {
                    await userSession.User.WriteRequest(filePackage, FILE_DATA_OUT);
                    foreach(var message in messages) {
                        await userSession.User.WriteRequest(message, MESSAGE_OUT);
                    }
                }
            }
        }

        public void RemoveClosedUserSession() {
            var closedSessions = _userSessions.Where(x => !x.User.IsOpen).ToList();
            foreach(var closedSession in closedSessions) {
                _userSessions.Remove(closedSession);
            }
        }
    }
}