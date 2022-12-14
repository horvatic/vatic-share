using System.Net.WebSockets;
using System.Text;
using System;

namespace UserSession {

    public class User
    {
        private readonly WebSocket _socket;
        private readonly byte[] _buffer;

        public bool IsOpen {
            get => _socket.State == WebSocketState.Open;
        }

        public User(WebSocket socket, int bufferSize) {
            _socket = socket;
            _buffer = new byte[1024 * 4];
        }

        public async Task<(string command, string package)> GetUserRequest() {
            if(!IsOpen) {
                return ("None", "");
            }

            var receiveResult = await _socket.ReceiveAsync(new ArraySegment<byte>(_buffer), CancellationToken.None);

            if(receiveResult.Count == 0) {
                return ("None", "");
            }

            var rawData = new ArraySegment<byte>(_buffer, 0, receiveResult.Count);
            if(rawData.Array == null) {
                throw new Exception();
            }

            var splitPackage = Encoding.UTF8.GetString(rawData).Split(" ", 2);
            return (splitPackage[0], splitPackage[1]);
        }

        public async Task WriteRequest(string rawPackage, string command) {
            if(!IsOpen) {
                return;
            }
            var package = Encoding.UTF8.GetBytes(command + rawPackage);
            await _socket.SendAsync(new ArraySegment<byte>(package, 0, package.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Close() {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", CancellationToken.None);
        }
    }
}
