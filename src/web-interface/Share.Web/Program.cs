using System.Text;
using Pipes;
using SharedConstants;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pipeBuilder = new PipeBuilder();
using var sessionInPipe = pipeBuilder.BuildSessionInPipe();
using var apiOutPipe = pipeBuilder.BuildWebApiOutPipe();

app.MapGet("/pushdata", async (string data, string sessionid) => {
    var encodedData = Encoding.ASCII.GetBytes(Constants.DATA_IN + sessionid + " " + data + "\n");
    await sessionInPipe.WriteAsync(encodedData, 0, encodedData.Length);
    return "Data Saved";
});

app.MapGet("/readdata", async (string sessionid) => {
    var encodedMessage = Encoding.ASCII.GetBytes($"{Constants.READ}{sessionid}\n");
    await sessionInPipe.WriteAsync(encodedMessage, 0, encodedMessage.Length);
    return await apiOutPipe.ReadLineAsync();
});

app.Run();
