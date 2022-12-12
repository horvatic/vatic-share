using MessageBus;
using Pipes;
using UserSession;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pipeBuilder = new PipeBuilder();
var cancellationTokenSource = new CancellationTokenSource();
var message = new Message();
using var sessionBlockDataInPipe = pipeBuilder.BuildSessionBlockDataInPipe();
using var apiBlockDataOutPipe = pipeBuilder.BuildWebApiBlockDataOutPipe();
using var sessionKeyDataInPipe = pipeBuilder.BuildSessionKeyDataInPipe();
using var apiKeyDataOutPipe = pipeBuilder.BuildWebApiKeyDataOutPipe();
var sessionSync = new SessionSync(sessionBlockDataInPipe, apiKeyDataOutPipe, apiBlockDataOutPipe);
var sessionSyncThread = new Thread(async() => {
    while(!cancellationTokenSource.Token.IsCancellationRequested) {
        await sessionSync.PushSessionData(cancellationTokenSource.Token);
        sessionSync.RemoveClosedUserSession();
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
        var userSession = new UserSessionModel(new User(webSocket, 1024 * 4), Guid.NewGuid().ToString());
        var session = new Session(userSession, sessionKeyDataInPipe, message);
        await sessionSync.SyncUserSession(userSession);
        await session.Run();
    }
    else
    {
        context.Response.StatusCode = 405;
    }
});

app.Run();
cancellationTokenSource.Cancel();