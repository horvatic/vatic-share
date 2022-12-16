using System.Text;
using MessageBus;

namespace UserSession {

    public class Session {
        private readonly UserSessionModel _userSession;
        private readonly FileStream _sessionInKeyDataPipe;
        private readonly FileStream _sessionInBlockDataPipe;
        private readonly StreamReader _apiOutBlockDataPipe;
        private readonly FileStream _sessionInCommandDataPipe;

        private readonly Message _message;

        private const string DATA_IN = "datain ";

        private const string FILE_KEY_IN = "filekey";

        private const string MESSAGE_IN = "message";

        private const string FILE_NAME_IN = "filename";
        private const string FILE_DATA_OUT = "filedata ";
        private const string READ = "read ";
        private const string CMD_IN = "command";
         private const string CMD_OUT = "commanddata ";

        public Session(UserSessionModel userSession, FileStream sessionInBlockDataPipe, FileStream sessionInKeyDataPipe, StreamReader apiOutBlockDataPipe, FileStream sessionInCommandDataPipe, Message message) {
            _userSession = userSession;
            _sessionInKeyDataPipe = sessionInKeyDataPipe;
            _message = message;
            _sessionInBlockDataPipe = sessionInBlockDataPipe;
            _apiOutBlockDataPipe = apiOutBlockDataPipe;
            _sessionInCommandDataPipe = sessionInCommandDataPipe;
        }

        public async Task Run() {
            while (_userSession.User.IsOpen) {
                var (command, request) = await _userSession.User.GetUserRequest();
                if(command == FILE_KEY_IN) {
                    await WriteToFile(request);
                }
                else if(command == MESSAGE_IN) {
                    _message.Push(request);
                }
                else if(command == FILE_NAME_IN) {
                    _userSession.SetOpenFile(request);
                    await SyncFile();
                    
                } else if( command == CMD_IN ) {
                    await SendCommand(request);
                }
            }
            await _userSession.User.Close();
        }

        private async Task SendCommand(string request) {
            var dataInEncoded = Encoding.UTF8.GetBytes(CMD_OUT);
            var sessionId = Encoding.UTF8.GetBytes(_userSession.SessionId);
            var endMessage = Encoding.UTF8.GetBytes("\n");
            var spaceData = Encoding.UTF8.GetBytes(" ");

            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(request));
            var requestPackage = new ArraySegment<byte>(Encoding.UTF8.GetBytes(base64), 0, base64.Length);
            if(requestPackage.Array == null) {
                return;
            }
            await _sessionInCommandDataPipe.WriteAsync(dataInEncoded, 0, dataInEncoded.Length);
            await _sessionInCommandDataPipe.WriteAsync(sessionId, 0, sessionId.Length);
            await _sessionInCommandDataPipe.WriteAsync(spaceData, 0, spaceData.Length);
            await _sessionInCommandDataPipe.WriteAsync(requestPackage.Array, 0, requestPackage.Count);
            await _sessionInCommandDataPipe.WriteAsync(endMessage, 0, endMessage.Length);
        }
        private async Task WriteToFile(string request) {
            if(_userSession.OpenFile == null) {
                return;
            }
            var keyInOffSet = FILE_KEY_IN.Length;
            var dataInEncoded = Encoding.UTF8.GetBytes(DATA_IN);
            var fileName = Encoding.UTF8.GetBytes(_userSession.OpenFile);
            var endMessage = Encoding.UTF8.GetBytes("\n");
            var spaceData = Encoding.UTF8.GetBytes(" ");

            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(request));
            var requestPackage = new ArraySegment<byte>(Encoding.UTF8.GetBytes(base64), 0, base64.Length);
            if(requestPackage.Array == null) {
                return;
            }
            await _sessionInKeyDataPipe.WriteAsync(dataInEncoded, 0, dataInEncoded.Length);
            await _sessionInKeyDataPipe.WriteAsync(fileName, 0, fileName.Length);
            await _sessionInKeyDataPipe.WriteAsync(spaceData, 0, spaceData.Length);
            await _sessionInKeyDataPipe.WriteAsync(requestPackage.Array, 0, requestPackage.Count);
            await _sessionInKeyDataPipe.WriteAsync(endMessage, 0, endMessage.Length);
        }

        private async Task SyncFile() {
            if(_userSession.OpenFile == null) {
                return;
            }

            var dataInEncoded = Encoding.UTF8.GetBytes(DATA_IN);
            var endMessage = Encoding.UTF8.GetBytes("\n");
            var spaceData = Encoding.UTF8.GetBytes(" ");
            var readEncoded = Encoding.UTF8.GetBytes(READ);
            var fileName = Encoding.UTF8.GetBytes(_userSession.OpenFile);

            await _sessionInBlockDataPipe.WriteAsync(readEncoded, 0, readEncoded.Length);
            await _sessionInBlockDataPipe.WriteAsync(fileName, 0, fileName.Length);
            await _sessionInBlockDataPipe.WriteAsync(endMessage, 0, endMessage.Length);

            var rawFilePackage = await _apiOutBlockDataPipe.ReadLineAsync() ?? "";
            var filePackage = "";
            var filePackageName = "";
            if(rawFilePackage != "") {
                var package = rawFilePackage.Split(" ", 2);
                filePackage = Encoding.UTF8.GetString(Convert.FromBase64String(package[1]));
                filePackageName = package[0];
            }

            await _userSession.User.WriteRequest(filePackage, FILE_DATA_OUT);
        }

    }
}
