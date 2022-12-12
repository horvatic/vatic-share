using System.Collections.Concurrent;

namespace MessageBus
{
    public class Message {
        private readonly ConcurrentQueue<string> _messages;

        public Message() {
            _messages = new ConcurrentQueue<string>();
        }

        public string Fetch() {
            if(_messages.TryDequeue(out var message)) {
                return message;
            }
            else {
                return "";
            }
        }

        public void Push(string message) {
            _messages.Enqueue(message);
        }

        public bool HasMessage() {
            return _messages.TryPeek(out _);
        }
    }
}