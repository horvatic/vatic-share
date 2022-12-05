using System.Text;
using Pipes;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pipeBuilder = new PipeBuilder();
using var sessionInPipe = pipeBuilder.BuildSessionInPipe();
using var apiOutPipe = pipeBuilder.BuildWebApiOutPipe();

app.MapGet("/", async () => {
    var message = Encoding.ASCII.GetBytes("Hello From Dotnet\n");
    await sessionInPipe.WriteAsync(message, 0, message.Length);
    return await apiOutPipe.ReadLineAsync();
});

app.Run();
