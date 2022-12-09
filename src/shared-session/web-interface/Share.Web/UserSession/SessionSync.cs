using System.Text;

namespace UserSession {
        public class SessionSync {
        private readonly List<User> _users;
        private readonly StreamReader _apiOutPipe;

        public SessionSync(StreamReader apiOutPipe) {
            _users = new List<User>();
            _apiOutPipe = apiOutPipe;
        }

        public void AddUser(User user) {
            _users.Add(user);
        }

        public async Task PushSessionData(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            var result = Encoding.UTF8.GetBytes(await _apiOutPipe.ReadLineAsync() ?? "");
            foreach(var user in _users) {
                if(user.IsOpen) {
                    await user.WriteRequest(result, result.Length);
                }
                else {
                    _users.Remove(user);
                }
            }
        }
    }
}