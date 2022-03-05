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
            new Movie{Title="The Shawshank Redemption", ReleaseYear=1994},
            new Movie{Title="The Green Mile", ReleaseYear=1999},
            new Movie{Title="Forrest Gump", ReleaseYear=1994},
            new Movie{Title="Avengers: Endgame", ReleaseYear=2019},
            new Movie{Title="Shrek", ReleaseYear=2001},
            new Movie{Title="The Pianist", ReleaseYear=2002},
            new Movie{Title="The Matrix", ReleaseYear=1999},
            new Movie{Title="Old No Name Kung Fu Movie", ReleaseYear=null}
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();
        }
    }
}