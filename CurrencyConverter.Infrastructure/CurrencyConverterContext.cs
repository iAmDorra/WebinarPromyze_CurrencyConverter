using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Infrastructure
{
    public class CurrencyConverterContext : DbContext
    {
        public DbSet<RateValue> Rates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CurrencyConverter.db");
        }
    }
}
