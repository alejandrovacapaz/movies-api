using Movies.Data.Model;

namespace Movies.Data.Seeding
{
    public static class DbSeedInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any() || context.Movies.Any())
                return;

            var users = new List<User>
            {
                new() { Name = "Alice" },
                new() { Name = "Bob" }
            };

            var movies = new List<Movie>
            {
                new() { Title = "The Matrix", YearOfRelease = 1999, RunningTime = 136, Genres = "Action,Sci-Fi" },
                new() { Title = "Inception", YearOfRelease = 2010, RunningTime = 148, Genres = "Action,Thriller" },
                new() { Title = "Interstellar", YearOfRelease = 2014, RunningTime = 169, Genres = "Drama,Sci-Fi" },
                new() { Title = "The Godfather", YearOfRelease = 1972, RunningTime = 175, Genres = "Crime,Drama" },
                new() { Title = "The Dark Knight", YearOfRelease = 2008, RunningTime = 152, Genres = "Action,Crime" },
                new() { Title = "Pulp Fiction", YearOfRelease = 1994, RunningTime = 154, Genres = "Crime,Drama" },
                new() { Title = "Fight Club", YearOfRelease = 1999, RunningTime = 139, Genres = "Drama" },
                new() { Title = "Forrest Gump", YearOfRelease = 1994, RunningTime = 142, Genres = "Drama,Romance" },
                new() { Title = "Gladiator", YearOfRelease = 2000, RunningTime = 155, Genres = "Action,Drama" },
                new() { Title = "Titanic", YearOfRelease = 1997, RunningTime = 195, Genres = "Romance,Drama" }
            };

            context.Users.AddRange(users);
            context.Movies.AddRange(movies);
            context.SaveChanges();

            var seededUsers = context.Users.ToList();
            var seededMovies = context.Movies.ToList();
            var rng = new Random();
            var ratings = new List<Rating>();

            foreach (var user in seededUsers)
            {
                foreach (var movie in seededMovies)
                {
                    ratings.Add(new Rating
                    {
                        UserId = user.Id,
                        MovieId = movie.Id,
                        Score = rng.Next(2, 6)
                    });
                }
            }

            context.Ratings.AddRange(ratings);
            context.SaveChanges();
        }
    }
}
