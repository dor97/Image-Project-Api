using Microsoft.EntityFrameworkCore;

namespace projectServer.Data
{
    public class PrebDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDBContext>());
            }
        }

        public static void SeedData(ApplicationDBContext? DBContext)
        {
            if(DBContext != null)
            {
                DBContext.Database.Migrate();
            }           
        }
    }
}
