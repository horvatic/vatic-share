using System.Text;
using MessageBus;

namespace UserSession {

    public class Session {
        private readonly UserSessionModel _userSession;
        private readonly FileStream _sessionInKeyDataPipe;

        private readonly Message _message;

        private const string DATA_IN = "datain ";

        private const string FILE_KEY_IN = "filekey";

        private const string MESSAGE_IN = "message";
        public Session(UserSessionModel userSession, FileStream sessionInKeyDataPipe, Message message) {
            _userSession = userSession;
            _sessionInKeyDataPipe = sessionInKeyDataPipe;
            _message = message;
        }

        public async Task Run() {
            var keyInOffSet = FILE_KEY_IN.Length;

            var dataInEncoded = Encoding.UTF8.GetBytes(DATA_IN);
            var fileName = Encoding.UTF8.GetBytes("filename");
            var endMessage = Encoding.UTF8.GetBytes("\n");
            var spaceData = Encoding.UTF8.GetBytes(" ");

            while (_userSession.User.IsOpen) {
                var (command, request) = await _userSession.User.GetUserRequest();
                if(command == FILE_KEY_IN) {
                    var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(request));
                    var requestPackage = new ArraySegment<byte>(Encoding.UTF8.GetBytes(base64), 0, base64.Length);
                    if(requestPackage.Array == null) {
                        continue ;
                    }
                    await _sessionInKeyDataPipe.WriteAsync(dataInEncoded, 0, dataInEncoded.Length);
                    await _sessionInKeyDataPipe.WriteAsync(fileName, 0, fileName.Length);
                    await _sessionInKeyDataPipe.WriteAsync(spaceData, 0, spaceData.Length);
                    await _sessionInKeyDataPipe.WriteAsync(requestPackage.Array, 0, requestPackage.Count);
                    await _sessionInKeyDataPipe.WriteAsync(endMessage, 0, endMessage.Length);
                }
                else if(command == MESSAGE_IN) {
                    _message.Push(request);
                }
            }
            await _userSession.User.Close();
        }
    }
}
