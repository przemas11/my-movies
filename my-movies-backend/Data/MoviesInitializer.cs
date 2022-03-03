using System.Linq;
using my_movies_backend.Models;

namespace my_movies_backend.Data
{
    public class MoviesInitializer
    {
        public static void Initialize(MoviesContext context)
        {
            context.Database.EnsureCreated();

            // Look for any movies.
            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            var movies = new Movie[]
            {
            new Movie{Title="The Shawshank Redemption", ReleaseDate=1994},
            new Movie{Title="The Green Mile", ReleaseDate=1999},
            new Movie{Title="Forrest Gump", ReleaseDate=1994},
            new Movie{Title="Avengers: Endgame", ReleaseDate=2019},
            new Movie{Title="Shrek", ReleaseDate=2001},
            new Movie{Title="The Pianist", ReleaseDate=2002},
            new Movie{Title="The Matrix", ReleaseDate=1999}
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();
        }
    }
}