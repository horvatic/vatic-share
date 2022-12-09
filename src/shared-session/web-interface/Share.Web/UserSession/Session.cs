using System.Text;

namespace UserSession {

    public class Session {
        private readonly UserSessionModel _userSession;
        private readonly FileStream _sessionInKeyDataPipe;

        private const string DATA_IN = "datain ";
        public Session(UserSessionModel userSession, FileStream sessionInKeyDataPipe) {
            _userSession = userSession;
            _sessionInKeyDataPipe = sessionInKeyDataPipe;
        }

        public async Task Run() {
            var dataInEncoded = Encoding.UTF8.GetBytes(DATA_IN);
            var fileName = Encoding.UTF8.GetBytes("filename");
            var endMessage = Encoding.UTF8.GetBytes("\n");
            var spaceData = Encoding.UTF8.GetBytes(" ");

            while (_userSession.User.IsOpen) {
                var request = await _userSession.User.GetUserRequest();
                if(request.Array == null) {
                    continue ;
                }
                await _sessionInKeyDataPipe.WriteAsync(dataInEncoded, 0, dataInEncoded.Length);
                await _sessionInKeyDataPipe.WriteAsync(fileName, 0, fileName.Length);
                await _sessionInKeyDataPipe.WriteAsync(spaceData, 0, spaceData.Length);
                await _sessionInKeyDataPipe.WriteAsync(request.Array, 0, request.Count);
                await _sessionInKeyDataPipe.WriteAsync(endMessage, 0, endMessage.Length);
            }
            await _userSession.User.Close();
        }
    }
}
