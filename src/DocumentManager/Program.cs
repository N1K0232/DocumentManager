using DocumentManager.BusinessLayer.Settings;
using Microsoft.Extensions.Options;
using TinyHelpers.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration, builder.Environment, builder.Host);

var app = builder.Build();
Configure(app, app.Environment, app.Services);

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, IHostBuilder host)
{
    var appSettings = services.ConfigureAndGet<AppSettings>(configuration, nameof(AppSettings));

    services.AddRequestLocalization(appSettings.SupportedCultures);
    services.AddRazorPages();
    services.AddWebOptimizer(minifyCss: true, minifyJavaScript: environment.IsProduction());
}

void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IServiceProvider services)
{
    var appSettings = services.GetRequiredService<IOptions<AppSettings>>().Value;
    environment.ApplicationName = appSettings.ApplicationName;

    app.UseHttpsRedirection();

    if (!environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseStatusCodePagesWithReExecute("/Errors/{0}");
    app.UseWebOptimizer();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
    });
}