using Pipes;
using UserSession;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pipeBuilder = new PipeBuilder();
using var sessionInPipe = pipeBuilder.BuildSessionInPipe();
using var apiOutPipe = pipeBuilder.BuildWebApiOutPipe();

var webSocketOptions = new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(120)
};
app.UseWebSockets(webSocketOptions);

app.Use(async (HttpContext context, Func<Task> next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var session = new Session(new User(webSocket, 1024 * 4), sessionInPipe, apiOutPipe, Guid.NewGuid().ToString());
        await session.Run();
    }
    else
    {
        context.Response.StatusCode = 405;
    }
});

app.Run();