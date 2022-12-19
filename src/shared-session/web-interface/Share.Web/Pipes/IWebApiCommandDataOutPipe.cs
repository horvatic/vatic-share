namespace Pipes {
    public interface IWebApiCommandDataOutPipe {
        Task<string?> ReadAsync();
    }
}