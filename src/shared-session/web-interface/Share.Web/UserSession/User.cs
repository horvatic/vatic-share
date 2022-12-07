using System.Net.WebSockets;
using System.Text;

namespace UserSession {

    public class User
    {
        private readonly WebSocket _socket;
        private readonly byte[] _buffer;

        public bool IsOpen {get; private set; }

        public User(WebSocket socket, int bufferSize) {
            _socket = socket;
            _buffer = new byte[1024 * 4];
            IsOpen = true;
        }

        public async Task<ArraySegment<byte>> GetUserRequest() {
            var receiveResult = await _socket.ReceiveAsync(new ArraySegment<byte>(_buffer), CancellationToken.None);
            IsOpen = !receiveResult.CloseStatus.HasValue;

            var base64 = System.Convert.ToBase64String(new ArraySegment<byte>(_buffer, 0, receiveResult.Count).Array);
            return new ArraySegment<byte>(Encoding.ASCII.GetBytes(base64), 0, base64.Length);
        }

        public async Task WriteRequest(byte[] buffer, int size) {
            var base64 = System.Convert.FromBase64String(Encoding.ASCII.GetString(buffer));
            await _socket.SendAsync(new ArraySegment<byte>(base64, 0, base64.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Close() {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", CancellationToken.None);
        }
    }
}
