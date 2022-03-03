using Microsoft.EntityFrameworkCore;
using my_movies_backend.Models;


namespace my_movies_backend.Data
{
    public class MoviesContext : DbContext
    {

        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("Movie");
        }
    }
}