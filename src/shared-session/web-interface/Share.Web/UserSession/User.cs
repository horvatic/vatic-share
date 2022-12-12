using System.Net.WebSockets;
using System.Text;
using System;

namespace UserSession {

    public class User
    {
        private readonly WebSocket _socket;
        private readonly byte[] _buffer;

        public bool IsOpen { get; private set; }

        public User(WebSocket socket, int bufferSize) {
            _socket = socket;
            _buffer = new byte[1024 * 4];
            IsOpen = true;
        }

        public async Task<(string, ArraySegment<byte>)> GetUserRequest() {
            var receiveResult = await _socket.ReceiveAsync(new ArraySegment<byte>(_buffer), CancellationToken.None);
            IsOpen = !receiveResult.CloseStatus.HasValue;

            var rawData = new ArraySegment<byte>(_buffer, 0, receiveResult.Count);
            if(rawData.Array == null) {
                throw new Exception();
            }

            var splitPackage = Encoding.UTF8.GetString(rawData).Split(" ", 2);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(splitPackage[1]));
            return (splitPackage[0], new ArraySegment<byte>(Encoding.UTF8.GetBytes(base64), 0, base64.Length));
        }

        public async Task WriteRequest(byte[] buffer, int size) {
            var base64 = Convert.FromBase64String(Encoding.UTF8.GetString(buffer));
            await _socket.SendAsync(new ArraySegment<byte>(base64, 0, base64.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Close() {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", CancellationToken.None);
        }
    }
}
