using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace UserSession {
    public class UserSessionStore {
        private static Mutex mut = new Mutex();
        private readonly List<UserSessionModel> _userSessions;

        public UserSessionStore() {
            _userSessions = new List<UserSessionModel>();
        }

        public void AddUser(UserSessionModel userSession) {
            mut.WaitOne();
            _userSessions.Add(userSession);
            mut.ReleaseMutex();
        }

        public void RemoveUser(UserSessionModel userSession) {
            mut.WaitOne();
            _userSessions.Remove(userSession);
            mut.ReleaseMutex();
        }

        public ReadOnlyCollection<UserSessionModel> GetUserList() {
            var readList = new List<UserSessionModel>();
            mut.WaitOne();
            foreach(var users in _userSessions) {
                readList.Add(users);
            }
            mut.ReleaseMutex();
            return new ReadOnlyCollection<UserSessionModel>(readList);
        }
    }
}