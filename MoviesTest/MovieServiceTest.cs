using Xunit;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Data.Model;
using Movies.Services.Imp;

namespace Movies.Tests
{
    public class MovieServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "MovieDbTest")
                .Options;

            var context = new AppDbContext(options);

            // Clear previous data
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed data
            var users = new List<User>
            {
                new() { Id = 1, Name = "Alice" },
                new() { Id = 2, Name = "Bob" }
            };

            var movies = new List<Movie>
            {
                new() { Id = 1, Title = "The Matrix", YearOfRelease = 1999, RunningTime = 136, Genres = "Action,Sci-Fi" },
                new() { Id = 2, Title = "Inception", YearOfRelease = 2010, RunningTime = 148, Genres = "Action,Thriller" },
            };

            var ratings = new List<Rating>
            {
                new() { Id = 1, MovieId = 1, UserId = 1, Score = 5 },
                new() { Id = 2, MovieId = 1, UserId = 2, Score = 4 },
                new() { Id = 3, MovieId = 2, UserId = 1, Score = 3 },
                new() { Id = 4, MovieId = 2, UserId = 2, Score = 4 },
            };

            context.Users.AddRange(users);
            context.Movies.AddRange(movies);
            context.Ratings.AddRange(ratings);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetTopRatedMovies_Test()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new MovieService(context);

            // Act
            var result = await service.GetTopRatedMovies();
            var list = result.ToList();

            // Assert
            Assert.Equal(2, list.Count);
            Assert.Equal("The Matrix", list[0].Title); 
            Assert.Equal("Inception", list[1].Title);
            Assert.Equal(4.5, list[0].AverageRating);
            Assert.Equal(3.5, list[1].AverageRating);
        }
    }
}
