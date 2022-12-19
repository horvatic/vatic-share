namespace Pipes
{
    public interface ISessionCommandInPipe {
        Task SendAsync(string message);
    }
}