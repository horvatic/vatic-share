using System.Text;

namespace UserSession {
    public class UserSessionModel {
        public User User { get; private set; }
        public string SessionId { get; private set; }

        public string? OpenFile { get; private set; }

        public UserSessionModel(User user, string sessionId) {
            User = user;
            SessionId = sessionId;
            OpenFile = null;
        }

        public void SetOpenFile(string filename) {
            OpenFile = filename;
        }
    }
}