using Pipes;
using UserSession;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pipeBuilder = new PipeBuilder();
var cancellationTokenSource = new CancellationTokenSource();
using var sessionInPipe = pipeBuilder.BuildSessionInPipe();
using var apiOutPipe = pipeBuilder.BuildWebApiOutPipe();
var sessionSync = new SessionSync(apiOutPipe);
var sessionSyncThread = new Thread(async() => {
    while(true) {
        await sessionSync.PushSessionData(cancellationTokenSource.Token);
    }
});
sessionSyncThread.Start();

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
        var user = new User(webSocket, 1024 * 4);
        var session = new Session(user, sessionInPipe, apiOutPipe, "1");
        sessionSync.AddUser(user);
        await session.Run();
    }
    else
    {
        context.Response.StatusCode = 405;
    }
});

app.Run();
cancellationTokenSource.Cancel();