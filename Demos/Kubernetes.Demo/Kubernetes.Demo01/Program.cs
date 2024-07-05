using Daily.Carp.Extension;
using Daily.Carp.Provider.Kubernetes;

var builder = WebApplication.CreateBuilder(args).InjectCarp();

// Add services to the container.

builder.Services.AddCarp().AddKubernetes(KubeDiscoveryType.EndPoint);

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


var app = builder.Build();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseCarp(options =>
{
    options.EnableAuthentication = true; //����Ȩ����֤
    options.CustomAuthenticationAsync.Add("Jwt", async () => //����� ��Jwt�� ��Ӧ���������ļ��е�PermissionsValidation�����е�ֵ
    {
        var flag = true;
        //��֤�߼�
        flag = false;
        //.....
        return await Task.FromResult(flag);
    });
});

app.MapControllers();

app.Run("http://*:6005");