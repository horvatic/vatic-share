namespace Pipes
{
    public interface ISessionBlockDataInPipe {
        Task SendReadAsync(string fileName);
        Task SendSaveAsync(string fileName);
    }
}