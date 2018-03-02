using System;

namespace Output.UnitTests.Models.Constructors
{
    public class SongDto
    {
        public SongDto()
        {
            Name = "Foo";
            AlbumName = "Bar";
            AlbumComposerName = "IO";
            AlbumReleaseDateDay = 1;
            AlbumReleaseDateMonth = 7;
            AlbumReleaseDateYear = 2018;
        }

        public string Name { get; set; }
        public string AlbumName { get; set; }
        public string AlbumComposerName { get; set; }
        public int AlbumReleaseDateDay { get; set; }
        public int AlbumReleaseDateMonth { get; set; }
        public int AlbumReleaseDateYear { get; set; }
    }

    public class Song
    {
        public Song(string name, Album album)
        {
            Name = name;
            Album = album;
        }

        public string Name { get; set; }
        public Album Album { get; set; }
    }

    public class Album
    {
        public Album(string name, Composer composer, DateTime releaseDate)
        {
            Name = name;
            Composer = composer;
            ReleaseDate = releaseDate;
        }

        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Composer Composer { get; set; }
    }

    public class Composer
    {
        public Composer(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}