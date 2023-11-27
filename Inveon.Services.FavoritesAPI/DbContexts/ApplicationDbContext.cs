using Inveon.Services.FavoritesAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace Inveon.Services.FavoritesAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }

    }
}
