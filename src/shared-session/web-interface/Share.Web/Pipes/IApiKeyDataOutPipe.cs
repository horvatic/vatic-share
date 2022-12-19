namespace Pipes {
    public interface IApiKeyDataOutPipe {
        Task<string?> ReadAsync();
    }
}