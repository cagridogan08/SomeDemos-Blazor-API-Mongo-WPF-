using DomainLibrary;
using Microsoft.EntityFrameworkCore;

namespace DataManagerLibrary.Context
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; } = null!;
    }
}
