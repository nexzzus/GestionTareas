using GestionTareas.Web.Data;
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
        
        return builder;
    }
}