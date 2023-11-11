using Microsoft.EntityFrameworkCore;

namespace WebAssignment.Models
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "BitcoinDatabase");
        }
        public DbSet<Bitcoin> BitcoinList { get; set; }
        public DbSet<Source> SourceList { get; set; }
    }
}
