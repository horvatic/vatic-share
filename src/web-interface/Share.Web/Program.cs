using System.IO.Pipes;
using System.Security.Principal;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () => {
    using FileStream fs = File.OpenWrite("/tmp/sessionInPipe");
    var message = Encoding.ASCII.GetBytes("Hello From Dotnet\n");
    await fs.WriteAsync(message, 0, message.Length);
});

app.Run();

