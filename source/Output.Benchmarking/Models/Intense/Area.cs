namespace Output.Benchmarking.Models.Intense
{
    public class Area
    {
        public Area(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public double Length { get; set; }
        public double Width { get; set; }
    }
}