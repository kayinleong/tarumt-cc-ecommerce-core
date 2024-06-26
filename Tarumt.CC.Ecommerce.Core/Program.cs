using Microsoft.Extensions.Options;
using Tarumt.CC.Ecommerce.Core.Extensions;
using Tarumt.CC.Ecommerce.Core.HostedService;
using Tarumt.CC.Ecommerce.Core.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabasesConfig(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddCorsConfig();
builder.Services.AddMvcConfig(builder.Environment);
builder.Services.AddLocalizationConfig();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddOpenIddictConfig(builder.Environment, builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddResponseCompressionConfig();
builder.Services.AddOpenApiConfig();
builder.Services.AddServiceConfig();

builder.Services.AddHostedService<DatabaseHostedService>();
builder.Services.AddHostedService<ServerSettingHostedService>();
builder.Services.AddHostedService<UserHostedService>();
builder.Services.AddHostedService<OpenIddictHostedService>();

WebApplication app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
app.UseCorsConfig();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

IOptions<RequestLocalizationOptions>? localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions!.Value);

app.UseStaticFiles();
app.UseOpenApiConfig();
app.UseExceptionConfig();
app.UseUserMiddleware();
app.MapControllerRoute(
    "areaRoute",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
