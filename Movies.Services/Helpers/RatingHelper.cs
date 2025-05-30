namespace Movies.Services.Helpers
{
    public static class RatingHelper
    {
        public static double RoundToNearestHalf(double value)
        {
            return Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2.0;
        }
    }
}
