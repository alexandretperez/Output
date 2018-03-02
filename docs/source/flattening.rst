.. include:: styles.txt

Flattening & Unflattening
=========================

Let's take this models for instance.

.. code-block:: c#

    public class Track
    {
        public Track(int id, string title, Album album)
        {
            Id = id;
            Title = title;
            Album = album;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Length { get; set; }

        public Album Album { get; set; }
        public Genre Genre { get; set; }
    }

    public class Album
    {
        public Album(int id, string title, Artist artist)
        {
            Id = id;
            Title = title;
            Artist = artist;
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public Artist Artist { get; set; }
    }

    public class Artist
    {
        public Artist(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


DTO representation:

.. code-block:: c#

    public class TrackDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Length { get; set; }
        
        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public int AlbumYear { get; set; }
        
        public int AlbumArtistId { get; set; }
        public string AlbumArtistName { get; set; }
        
        public int? GenreId { get; set; }
        public string GenreName { get; set; }	
    }

Flattening
----------

Flattening objects by hand is a repetitive and boring task.

.. code-block:: c#

    var trackDto = new TrackDto
    {
        Id = track.Id,
        Length = track.Length,
        Title = track.Title,

        AlbumId = track.Album?.Id,
        AlbumTitle = track.Album?.Title,
        AlbumYear = track.Album?.Year,
        
        AlbumArtistId = track.Album?.Artist?.Id,
        AlbumArtistName = track.Album?.Artist?.Name,

        GenreId = track.Genre?.Id,
        GenreName = track.Genre?.Name
    }

With *Output* this task is dead easy.

.. code-block:: c#

    var trackDto = mapper.Map<TrackDto>(track);


Unflattening
------------

Unflattening is also possible and even with non-public constructors the *Ouput* is able to determine which constructor to use *based on your DTO representation*. 

.. code-block:: C#

    var track = mapper.Map<Track>(trackDto);

Read more about :underline:`ConstructorResolver` in :doc:`resolvers`.