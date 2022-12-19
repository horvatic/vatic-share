using MessageBus;
using Pipes;
using UserSession;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pipeBuilder = new PipeBuilder();
var cancellationTokenSource = new CancellationTokenSource();
var message = new Message();
var userSessionStore = new UserSessionStore();
using var sessionBlockDataInPipe = new SessionBlockDataInPipe();
using var apiBlockDataOutPipe = new ApiBlockDataOutPipe();
using var sessionKeyDataInPipe = new SessionKeyDataInPipe();
using var apiKeyDataOutPipe = new ApiKeyDataOutPipe();
using var sessionCommandInPipe = new SessionCommandInPipe();
using var webApiCommandDataOutPipe = pipeBuilder.BuildWebApiCommandDataOutPipe();
var sessionSync = new SessionSync(apiKeyDataOutPipe, webApiCommandDataOutPipe, message, userSessionStore);
var fileSessionSyncThread = new Thread(async() => {
    while(!cancellationTokenSource.Token.IsCancellationRequested) {
        await sessionSync.PushFileSessionData(cancellationTokenSource.Token);
        sessionSync.RemoveClosedUserSession();
    }
});
var messageSessionSyncThread = new Thread(async() => {
    while(!cancellationTokenSource.Token.IsCancellationRequested) {
        await sessionSync.PushMessageSessionData(cancellationTokenSource.Token);
    }
});
var commandSessionSyncThread = new Thread(async() => {
    while(!cancellationTokenSource.Token.IsCancellationRequested) {
        await sessionSync.PushCommandSessionData(cancellationTokenSource.Token);
    }
});

commandSessionSyncThread.Start();
fileSessionSyncThread.Start();
messageSessionSyncThread.Start();

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
        var session = new Session(userSession, sessionBlockDataInPipe, sessionKeyDataInPipe, apiBlockDataOutPipe, sessionCommandInPipe, message);
        sessionSync.SyncUserSession(userSession);
        await session.Run();
    }
    else
    {
        context.Response.StatusCode = 405;
    }
});

app.Run();
cancellationTokenSource.Cancel();