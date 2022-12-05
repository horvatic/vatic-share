using System.Text;
using Pipes;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pipeBuilder = new PipeBuilder();
using var sessionInPipe = pipeBuilder.BuildSessionInPipe();
using var apiOutPipe = pipeBuilder.BuildWebApiOutPipe();

app.MapGet("/pushdata", async (string data) => {
    var encodedData = Encoding.ASCII.GetBytes("datain " + data + "\n");
    await sessionInPipe.WriteAsync(encodedData, 0, encodedData.Length);
    return "Data Saved";
});

app.MapGet("/readdata", async () => {
    var encodedMessage = Encoding.ASCII.GetBytes("read \n");
    await sessionInPipe.WriteAsync(encodedMessage, 0, encodedMessage.Length);
    return await apiOutPipe.ReadLineAsync();
});

app.Run();
