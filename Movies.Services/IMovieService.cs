using Movies.Data.Dtos;

namespace Movies.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> SearchMovies(string? title, int? year, string? genres);
        Task<IEnumerable<MovieDto>> GetTopRatedMovies();
        Task<IEnumerable<MovieDto>> GetTopRatedMoviesByUser(int userId);
        Task<(bool success, string message)> RateMovie(int movieId, int userId, double score);
    }
}
