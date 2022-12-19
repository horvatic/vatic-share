using System.Text;
using MessageBus;
using Pipes;

namespace UserSession {
    public class SessionSync {
        private readonly UserSessionStore _userSessionStore;
        private readonly IApiKeyDataOutPipe _apiOutKeyDataPipe;
        private readonly IWebApiCommandDataOutPipe _apiOutCommandDataPipe;

        private readonly Message _message;

        private const string DATA_IN = "datain ";

        private const string READ = "read ";
        private const string FILE_DATA_OUT = "filedata ";

        private const string MESSAGE_OUT = "message ";

        private const string COMMAND_OUT = "commanddata ";

        public SessionSync(IApiKeyDataOutPipe apiOutKeyDataPipe, IWebApiCommandDataOutPipe apiOutCommandDataPipe, Message message, UserSessionStore userSessionStore) {
            _userSessionStore = userSessionStore;
            _apiOutKeyDataPipe = apiOutKeyDataPipe;
            _apiOutCommandDataPipe = apiOutCommandDataPipe;
            _message = message;
        }

        public void SyncUserSession(UserSessionModel userSession) {
            _userSessionStore.AddUser(userSession);
        }

        public async Task PushCommandSessionData(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            var rawCommandPackage = await _apiOutCommandDataPipe.ReadAsync() ?? "";
            var commandPackage = "";
            var sessionId = "";
            if(rawCommandPackage != "") {
                var package = rawCommandPackage.Split(" ", 2);
                commandPackage = Encoding.UTF8.GetString(Convert.FromBase64String(package[1]));
                sessionId = package[0];
            }
            foreach(var userSession in _userSessionStore.GetUserList()) {
                if(userSession.User.IsOpen && sessionId == userSession.SessionId) {
                    await userSession.User.WriteRequest(commandPackage, COMMAND_OUT);
                }
            }
        }

        public async Task PushFileSessionData(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            var rawFilePackage = await _apiOutKeyDataPipe.ReadAsync() ?? "";
            var filePackage = "";
            var filePackageName = "";
            if(rawFilePackage != "") {
                var package = rawFilePackage.Split(" ", 2);
                filePackage = Encoding.UTF8.GetString(Convert.FromBase64String(package[1]));
                filePackageName = package[0];
            }
            foreach(var userSession in _userSessionStore.GetUserList()) {
                if(userSession.User.IsOpen && filePackageName == userSession.OpenFile) {
                    await userSession.User.WriteRequest(filePackage, FILE_DATA_OUT);
                }
            }
        }

        public async Task PushMessageSessionData(CancellationToken cancellationToken) {
            if(!_message.HasMessage()) {
                return;
            }
            
            var messages = new List<string>();
            while(_message.HasMessage()) {
                messages.Add(_message.Fetch());
            }       
            foreach(var userSession in _userSessionStore.GetUserList()) {
                if(userSession.User.IsOpen) {
                    foreach(var message in messages) {
                        await userSession.User.WriteRequest(message, MESSAGE_OUT);
                    }
                }
            }
        }

        public void RemoveClosedUserSession() {
            var closedSessions = _userSessionStore.GetUserList().Where(x => !x.User.IsOpen).ToList();
            foreach(var closedSession in closedSessions) {
                _userSessionStore.RemoveUser(closedSession);
            }
        }
    }
}