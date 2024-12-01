using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TournamentApiContext>();

            await dbContext.Database.MigrateAsync();
            await SeedData.SeedAsync(dbContext);
        }
    }
}
