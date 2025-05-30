using Microsoft.AspNetCore.Mvc;
using Movies.Data.Model;
using Movies.Services;

namespace Movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? title, [FromQuery] int? year, [FromQuery] string? genres)
        {
            try
            {
                var result = await _movieService.SearchMovies(title, year, genres);
                return result.Any() ? Ok(result) : NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpGet("top-rated")]
        public async Task<IActionResult> TopRated()
        {
            try
            {
                var result = await _movieService.GetTopRatedMovies();
                return result.Any() ? Ok(result) : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

        [HttpGet("user/{userId}/top-rated")]
        public async Task<IActionResult> TopRatedByUser(int userId)
        {
            try
            {
                var result = await _movieService.GetTopRatedMoviesByUser(userId);
                return result.Any() ? Ok(result) : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }

        [HttpPut("{movieId}/ratings")]
        public async Task<IActionResult> Rate(int movieId, [FromBody] Rating rating)
        {
            var (success, message) = await _movieService.RateMovie(movieId, rating.UserId, rating.Score);
            if (!success)
                return message.Contains("Invalid") ? BadRequest(message) : NotFound(message);

            return Ok(message);
        }
    }
}
