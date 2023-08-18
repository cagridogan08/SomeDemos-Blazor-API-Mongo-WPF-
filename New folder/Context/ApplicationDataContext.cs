using DomainLibrary;
using Microsoft.EntityFrameworkCore;

namespace WpfAppWithRedisCache.Context
{
    internal class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {

        }
        public virtual DbSet<Product> Products { get; set; } = null!;
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(App.GetRequiredService<IConfiguration>()!.GetConnectionString("DefaultConnection"));
        //}
    }
}
