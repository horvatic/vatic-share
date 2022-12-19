namespace Pipes {
    public interface ISessionKeyDataInPipe {
        Task SendAsync(string message);
    }
}