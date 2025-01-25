using Microsoft.EntityFrameworkCore;

namespace IoTBlockchain.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
        {
            
        }

        public DbSet<Block> Blocks { get; set; }
        public DbSet<Data> Datas { get; set; }
    }
}
