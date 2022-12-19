namespace Pipes {
    public interface ISessionKeyDataInPipe {
        Task SendAsync(string openFile, string message);
    }
}