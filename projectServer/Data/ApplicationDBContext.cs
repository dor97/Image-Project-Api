using Microsoft.EntityFrameworkCore;
using projectServer.Models;

namespace projectServer.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
                
        }

        public DbSet<SampleImageModel> ImagesUpload { get; set; }

        public DbSet<UserModel> UsersModel { get; set; }
    }
}
