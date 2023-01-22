using Microsoft.EntityFrameworkCore;

namespace FoodRatingApi.Entities
{
    public class FoodRatingDbContext : DbContext
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodRating> FoodRatings { get; set; }

        public FoodRatingDbContext(DbContextOptions<FoodRatingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
