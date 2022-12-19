namespace Pipes
{
    public interface ISessionBlockDataInPipe {
        Task SendAsync(string message);
    }
}