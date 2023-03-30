using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Daily.Carp.Extension;

var builder = WebApplication.CreateBuilder(args).InjectCarp();

// Add services to the container.


builder.Configuration.AddApollo(builder.Configuration.GetSection("Apollo"))
    .AddDefault()
    .AddNamespace(ConfigConsts.NamespaceApplication);

builder.Services.AddCarp().AddKubernetes();

builder.Services.AddControllers();


var app = builder.Build();

app.UseStaticFiles();

app.UseCarp(options =>
{
    options.AuthenticationCenter = builder.Configuration["AuthenticationCenter_Url"];  //��֤���ĵĵ�ַ
    options.Enable = true; //����Ȩ����֤
});

app.MapControllers();

app.Run("http://*:6005");