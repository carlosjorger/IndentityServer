using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using indentityServer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //builder.Host.UseSerilog((ctx, lc) => lc
    //    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    //    .Enrich.FromLogContext()
    //    .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddIdentityServer(options =>{
        options.Events.RaiseErrorEvents= true;
        options.Events.RaiseInformationEvents= true;
        options.Events.RaiseFailureEvents= true;
        options.Events.RaiseSuccessEvents= true;
        options.EmitStaticAudienceClaim= true;
    })
    .AddTestUsers(TestUsers.Users)
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    ;

    var app = builder.Build();

    app.UseIdentityServer();
    
    app.MapGet("/", () => "Hello World");
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}