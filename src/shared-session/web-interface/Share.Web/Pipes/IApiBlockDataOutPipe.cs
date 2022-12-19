namespace Pipes {
    public interface IApiBlockDataOutPipe {
        Task<string?> ReadAsync();
    }
}