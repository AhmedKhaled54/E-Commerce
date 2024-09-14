using Core.Data;
using Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Helper
{
    public  class ApplySeeding
    {
        public static async Task ApplySeedData(WebApplication application)
        {
            using (var scope =application.Services.CreateScope())
            {

                var services=scope.ServiceProvider;
                var logger = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context=services.GetRequiredService<AppDBContext>();
                    await context.Database.MigrateAsync();
                    await AppSedd.SeedData(context,logger);

                }
                catch (Exception ex)
                {
                    var log = logger.CreateLogger<AppSedd>();
                    log.LogError(ex.Message);

                }

            }
        }

    }
}
