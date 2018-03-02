using System.Collections.Generic;

namespace Output.UnitTests.Models.RealWorld
{
    public class Director
    {
        public Director()
        {
            Name = "Peter Jackson";
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