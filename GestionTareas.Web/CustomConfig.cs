using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using GestionTareas.Web.Core;
using GestionTareas.Web.Data;
using GestionTareas.Web.Services.Abstractions;
using GestionTareas.Web.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas.Web;

public static class CustomConfig
{
    public static WebApplicationBuilder AddCustomConfig(this WebApplicationBuilder builder)
    {
        // DataContext
        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
        });
        // AutoMapper
        builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
        
        // ToastNoty
        builder.Services.AddNotyf(config =>
        {
            config.DurationInSeconds = 3;
            config.IsDismissable = true;
            config.Position = NotyfPosition.BottomRight;
        });
        
        // Services
        builder.Services.AddScoped<ITaskService, TaskService>();
        
        return builder;
    }

    public static WebApplication AddCustomWebAppConfig(this WebApplication app)
    {
        app.UseNotyf();
        
        return app;
    }
}