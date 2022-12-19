namespace Pipes
{
    public interface ISessionCommandInPipe {
        Task SendAsync(string sessionId, string message);
    }
}