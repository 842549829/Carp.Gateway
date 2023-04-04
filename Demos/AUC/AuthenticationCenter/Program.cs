
using AuthenticationCenter.DataInit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential() //���������û��֤�����ʹ�õĿ���������
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryIdentityResources(Config.IdentityResources);

var app = builder.Build();

app.UseIdentityServer();
app.MapGet("/", () => "Hello World!");

app.Run();