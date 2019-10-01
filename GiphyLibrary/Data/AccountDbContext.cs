using GiphyLibrary.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GiphyLibrary.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<StoredGiphy> Giphies { get; set; }
    }
}
