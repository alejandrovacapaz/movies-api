namespace Movies.Data.Model
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public string Genres { get; set; }
        public List<Rating> Ratings { get; set; } = new();
    }
}
