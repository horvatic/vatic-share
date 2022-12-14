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

        

        private const string FILE_KEY_IN = "filekey";

        private const string MESSAGE_IN = "message";

        private const string FILE_NAME_IN = "filename";
        private const string FILE_DATA_OUT = "filedata ";
        private const string FILE_SAVE = "save";
        
        private const string CMD_IN = "command";

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
                } else if ( command == FILE_SAVE ) {
                    await SaveFile();
                }
            }
            await _userSession.User.Close();
        }

        private async Task SaveFile() {
            if(_userSession.OpenFile == null) {
                return;
            }
            await _sessionInBlockDataPipe.SendSaveAsync(_userSession.OpenFile);
        }

        private async Task SendCommand(string request) {
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(request));
            await _sessionInCommandDataPipe.SendAsync(_userSession.SessionId, base64);
        }
        
        private async Task WriteToFile(string request) {
            if(_userSession.OpenFile == null) {
                return;
            }
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(request));

            await _sessionInKeyDataPipe.SendAsync(_userSession.OpenFile, base64);
        }

        private async Task SyncFile() {
            if(_userSession.OpenFile == null) {
                return;
            }

            await _sessionInBlockDataPipe.SendReadAsync(_userSession.OpenFile);

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
