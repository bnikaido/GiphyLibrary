using GiphyLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GiphyLibrary.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {
        }

        public DbSet<StoredGiphy> Giphies { get; set; }
    }
}
