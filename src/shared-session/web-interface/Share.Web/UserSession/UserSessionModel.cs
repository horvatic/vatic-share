using System.Text;

namespace UserSession {
    public class UserSessionModel {
        public User User { get; private set; }
        public byte[] SessionId { get; private set; }

        public UserSessionModel(User user, string sessionId) {
            User = user;
            SessionId = Encoding.UTF8.GetBytes(sessionId);
        }
    }
}