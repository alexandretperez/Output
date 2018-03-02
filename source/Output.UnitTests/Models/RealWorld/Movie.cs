using System.Collections.Generic;

namespace Output.UnitTests.Models.RealWorld
{
    /// <summary>
    /// Big object with recursion tests
    /// </summary>
    public class Movie
    {
        public Movie()
        {
            Name = "The Lord of the Rings: The Fellowship of the Ring";
            Genres = new List<MovieGenre>
            {
                MovieGenre.CreateAndAddMovie("Adventure", this),
                MovieGenre.CreateAndAddMovie("Fantasy", this),
                MovieGenre.CreateAndAddMovie("Drama", this)
            };

            Actors = new[] {
                new Actor("Elijah Wood"),
                new Actor("Ian McKellen"),
                new Actor("Orlando Bloom"),
                new Actor("Sean Astin"),
                new Actor("Christopher Lee"),
                new Actor("Viggo Mortensen"),
                new Actor("Liv Tyler"),
                new Actor("Cate Blanchett")
            };

            Director = new Director();
            Director.Movies.Add(this);
        }

        public string Name { get; set; }
        public List<MovieGenre> Genres { get; set; }
        public Director Director { get; set; }
        public Actor[] Actors { get; set; }
    }

    public class MovieDto
    {
        public string Name { get; set; }
        public List<MovieGenreDto> Genres { get; set; }
        public DirectorDto Director { get; set; }
        public ActorDto[] Actors { get; set; }
    }
}