using System.Collections.Generic;

namespace Output.Benchmarking.Models.Complex
{
    public class Movie
    {
        private Movie()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public List<MovieGenre> Genres { get; set; }
        public Director Director { get; set; }
        public Actor[] Actors { get; set; }
        public SoundTrack[] SoundTracks { get; set; }

        public static Movie LordOfTheRings()
        {
            var movie = new Movie
            {
                Id = 1,
                Name = "The Lord of the Rings: The Fellowship of the Ring",
                Year = 2001,
                Director = new Director()
            };
            movie.Genres = new List<MovieGenre>
            {
                MovieGenre.CreateAndAddMovie("Adventure", movie),
                MovieGenre.CreateAndAddMovie("Fantasy", movie),
                MovieGenre.CreateAndAddMovie("Drama", movie)
            };

            movie.Actors = new[] {
                new Actor("Elijah Wood"),
                new Actor("Ian McKellen"),
                new Actor("Orlando Bloom"),
                new Actor("Sean Astin"),
                new Actor("Christopher Lee"),
                new Actor("Viggo Mortensen"),
                new Actor("Liv Tyler"),
                new Actor("Cate Blanchett")
                // ...
            };

            var howard = new Composer("Howard Shore");
            movie.SoundTracks = new[]
            {
                new SoundTrack("The Prophecy", 213, howard),
                new SoundTrack("Concerning Hobbits", 153, howard),
                new SoundTrack("The Treason of Isengard", 240, howard),
                new SoundTrack("Many Meetings", 183, howard),
                new SoundTrack("May It Be", 251, new Composer("Enya"))
                // ...
            };

            movie.Director.Name = "Peter Jackson";
            movie.Director.Movies.Add(movie);
            return movie;
        }

        public static Movie TheMatrix()
        {
            var movie = new Movie
            {
                Id = 2,
                Name = "The Matrix",
                Year = 1999,
                Director = new Director()
            };
            movie.Genres = new List<MovieGenre>
            {
                MovieGenre.CreateAndAddMovie("Action", movie),
                MovieGenre.CreateAndAddMovie("Sci-Fi", movie)
            };

            movie.Actors = new[] {
                new Actor("Keanu Reeves"),
                new Actor("Laurence Fishburne"),
                new Actor("Carrie-Anne Moss"),
                new Actor("Hugo Weaving"),
                new Actor("Gloria Foster"),
                new Actor("Joe Pantoliano"),
                new Actor("Marcus Chong")
                // ...
            };

            var howard = new Composer("Howard Shore");
            movie.SoundTracks = new[]
            {
                new SoundTrack("Wake up", 604, new Composer("Rage Against The Machine")),
                new SoundTrack("Rock Is Dead", 311, new Composer("Marilyn Manson")),
                new SoundTrack("Spybreak (Short One)", 401, new Composer("Propellerheads")),
                new SoundTrack("Mindfields", 540, new Composer("The Prodigy"))
                // ...
            };

            movie.Director.Name = "The Wachowski Brothers";
            movie.Director.Movies.Add(movie);
            return movie;
        }

        public static Movie TheRevenant()
        {
            var movie = new Movie
            {
                Id = 3,
                Name = "The Revenant",
                Year = 2015,
                Director = new Director()
            };
            movie.Genres = new List<MovieGenre>
            {
                MovieGenre.CreateAndAddMovie("Adventure", movie),
                MovieGenre.CreateAndAddMovie("Drama", movie),
                MovieGenre.CreateAndAddMovie("History", movie)
            };

            movie.Actors = new[] {
                new Actor("Leonardo DiCaprio"),
                new Actor("Tom Hardy"),
                new Actor("Domhnall Gleeson"),
                new Actor("Will Poulter"),
                new Actor("Forrest Goodluck"),
                new Actor("Paul Anderson"),
                new Actor("Kristoffer Joner")
                // ...
            };

            var howard = new Composer("Howard Shore");
            movie.SoundTracks = new[]
            {
                new SoundTrack("The Revenant Main Theme", 241, new Composer("Ryuichi Sakamoto")),
                new SoundTrack("Hawk Punished", 214, new Composer("Alva Noto & Bryce Dessner")),
                new SoundTrack("Carrying Glass", 307, new Composer("Ryuichi Sakamoto & Alva Noto")),
                new SoundTrack("Powaqa Rescue", 535, new Composer("Ryuichi Sakamoto, Alva Noto & Bryce Dessner")),
                new SoundTrack("Imagining Buffalo", 239,new Composer("Bryce Dessner"))
                // ...
            };

            movie.Director.Name = "Alejandro G. Iñárritu";
            movie.Director.Movies.Add(movie);
            return movie;
        }
    }

    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public List<MovieGenreDto> Genres { get; set; }
        public DirectorDto Director { get; set; }
        public ActorDto[] Actors { get; set; }
        public SoundTrackDto[] SoundTracks { get; set; }
    }
}