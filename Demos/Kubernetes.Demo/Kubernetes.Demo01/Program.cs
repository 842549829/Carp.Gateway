using Daily.Carp.Extension;
using Daily.Carp.Provider.Kubernetes;
using KubeClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var path = Path.Combine(AppContext.BaseDirectory, "admin.conf");
// ͨ�������ļ�
KubeClientOptions clientOptions = K8sConfig.Load(path).ToKubeClientOptions(
    defaultKubeNamespace: "default"
);

builder.Services.AddCarp().AddKubernetes(KubeDiscoveryType.EndPoint, clientOptions);

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