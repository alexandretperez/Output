using System.Collections.Generic;

namespace Output.UnitTests.Models.RealWorld
{
    public class MovieGenre
    {
        public MovieGenre(string name)
        {
            Name = name;
            Movies = new List<Movie>();
        }

        public string Name { get; set; }
        public List<Movie> Movies { get; set; }

        public static MovieGenre CreateAndAddMovie(string name, Movie movie)
        {
            var m = new MovieGenre(name);
            m.Movies.Add(movie);
            return m;
        }
    }

    public class MovieGenreDto
    {
        public string Name { get; set; }
        public List<MovieDto> Movies { get; set; }
    }
}