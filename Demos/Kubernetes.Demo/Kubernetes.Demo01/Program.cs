using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Daily.Carp.Extension;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args).InjectCarp();

// Add services to the container.


builder.Configuration.AddApollo(builder.Configuration.GetSection("Apollo"))
    .AddDefault()
    .AddNamespace(ConfigConsts.NamespaceApplication);


builder.Services.AddCarp().AddKubernetes();

builder.Services.AddControllers();

#region ֧�ֿ���  ���е�Api��֧�ֿ���

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.SetIsOriginAllowed((x) => true)
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

#endregion ֧�ֿ���  ���е�Api��֧�ֿ���

builder.WebHost.UseKestrel(options =>
{
    var x509ca = new X509Certificate2(File.ReadAllBytes(@"jtys.cqyt.petrochina.pfx"));
    options.ListenAnyIP(6005, listenOptions => listenOptions.UseHttps(x509ca));
});

var app = builder.Build();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseCarp(options =>
{
    options.AuthenticationCenter = builder.Configuration["AuthenticationCenter_Url"];  //��֤���ĵĵ�ַ
    options.Enable = true; //����Ȩ����֤
});

app.MapControllers();

app.Run();