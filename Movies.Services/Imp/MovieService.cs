using Movies.Data.Dtos;
using Movies.Data.Model;
using Movies.Data;
using Microsoft.EntityFrameworkCore;
using Movies.Services.Helpers;

namespace Movies.Services.Imp
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDto>> SearchMovies(string? title, int? year, string? genres)
        {
            if (string.IsNullOrWhiteSpace(title) && !year.HasValue && string.IsNullOrWhiteSpace(genres))
                throw new ArgumentException("No valid search criteria provided");

            var query = _context.Movies.Include(m => m.Ratings).AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(m => m.Title.Contains(title));

            if (year.HasValue)
                query = query.Where(m => m.YearOfRelease == year);

            if (!string.IsNullOrWhiteSpace(genres))
            {
                var genreList = genres.Split(',').Select(g => g.Trim());
                query = query.Where(m => genreList.Any(g => m.Genres.Contains(g)));
            }

            var movies = await query.ToListAsync();
            return movies.Select(MapToDto);
        }

        public async Task<IEnumerable<MovieDto>> GetTopRatedMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Ratings)
                .Where(m => m.Ratings.Any())
                .OrderByDescending(m => m.Ratings.Average(r => r.Score))
                .Take(5)
                .ToListAsync();

            return movies.Select(MapToDto);
        }


        public async Task<IEnumerable<MovieDto>> GetTopRatedMoviesByUser(int userId)
        {
            var user = await _context.Users.Include(u => u.Ratings).ThenInclude(r => r.Movie).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return Enumerable.Empty<MovieDto>();

            return user.Ratings
                .OrderByDescending(r => r.Score)
                .Take(5)
                .Select(r => MapToDto(r.Movie));
        }

        public async Task<(bool success, string message)> RateMovie(int movieId, int userId, double score)
        {
            if (score < 0 || score > 5)
                return (false, "Invalid rating value.");

            var movie = await _context.Movies.FindAsync(movieId);
            var user = await _context.Users.FindAsync(userId);

            if (movie == null || user == null)
                return (false, "Movie or user not found.");

            var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            if (rating != null)
            {
                rating.Score = score;
            }
            else
            {
                _context.Ratings.Add(new Rating { MovieId = movieId, UserId = userId, Score = score });
            }

            await _context.SaveChangesAsync();
            return (true, "Rating saved.");
        }

        private MovieDto MapToDto(Movie m)
        {
            return new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                YearOfRelease = m.YearOfRelease,
                RunningTime = m.RunningTime,
                Genres = m.Genres,
                AverageRating = m.Ratings.Any()
                ? RatingHelper.RoundToNearestHalf(m.Ratings.Average(r => r.Score))
                : 0
            };
        }
    }

}
