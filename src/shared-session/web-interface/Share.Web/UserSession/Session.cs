using System.Text;
using MessageBus;
using Pipes;

namespace UserSession {

    public class Session {
        private readonly UserSessionModel _userSession;
        private readonly ISessionKeyDataInPipe _sessionInKeyDataPipe;
        private readonly ISessionBlockDataInPipe _sessionInBlockDataPipe;
        private readonly IApiBlockDataOutPipe _apiOutBlockDataPipe;
        private readonly ISessionCommandInPipe _sessionInCommandDataPipe;

        private readonly Message _message;

        private const string DATA_IN = "datain ";

        private const string FILE_KEY_IN = "filekey";

        private const string MESSAGE_IN = "message";

        private const string FILE_NAME_IN = "filename";
        private const string FILE_DATA_OUT = "filedata ";
        private const string READ = "read ";
        private const string CMD_IN = "command";
         private const string CMD_OUT = "commanddata ";

        public Session(UserSessionModel userSession, ISessionBlockDataInPipe sessionInBlockDataPipe, ISessionKeyDataInPipe sessionInKeyDataPipe, IApiBlockDataOutPipe apiOutBlockDataPipe, ISessionCommandInPipe sessionInCommandDataPipe, Message message) {
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
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(request));
            await _sessionInCommandDataPipe.SendAsync(CMD_OUT + _userSession.SessionId + " " + base64);
        }
        
        private async Task WriteToFile(string request) {
            if(_userSession.OpenFile == null) {
                return;
            }
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(request));

            await _sessionInKeyDataPipe.SendAsync(DATA_IN + _userSession.OpenFile + " " + base64);
        }

        private async Task SyncFile() {
            if(_userSession.OpenFile == null) {
                return;
            }

            await _sessionInBlockDataPipe.SendAsync(READ + _userSession.OpenFile);

            var rawFilePackage = await _apiOutBlockDataPipe.ReadAsync() ?? "";
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
