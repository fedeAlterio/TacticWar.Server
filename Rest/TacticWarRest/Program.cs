using TacticWar.Rest;

var appBuilder = WebApplication.CreateBuilder();
var startup = new Startup(appBuilder.Configuration);

startup.ConfigureServices(appBuilder.Services);
var app = appBuilder.Build();
startup.Configure(app, appBuilder.Environment);

await app.RunAsync();