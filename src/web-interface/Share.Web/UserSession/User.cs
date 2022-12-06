using System.Net.WebSockets;

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
            if(!IsOpen) {

            }
            return new ArraySegment<byte>(_buffer, 0, receiveResult.Count);
        }

        public async Task WriteRequest(byte[] buffer, int size) {
            await _socket.SendAsync(new ArraySegment<byte>(buffer, 0, size), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Close() {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", CancellationToken.None);
        }
    }
}
