using System.Collections.Generic;

namespace Output.Benchmarking.Models.Complex
{
    public class Director
    {
        public Director()
        {
            Movies = new List<Movie>();
        }

        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }

    public class DirectorDto
    {
        public string Name { get; set; }
        public List<MovieDto> Movies { get; set; }
    }
}