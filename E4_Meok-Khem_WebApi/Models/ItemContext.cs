 using Microsoft.EntityFrameworkCore;
namespace E4_Meok_Khem_WebApi.Models
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options)
            : base(options)
        {
        }

        public DbSet<ItemModel> TodoItems { get; set; } = null!;
    }
}
