namespace Movies.Data.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Rating> Ratings { get; set; } = new();
    }
}
