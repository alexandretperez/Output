using System;

namespace Output.UnitTests.Models.Constructors
{
    public class Note
    {
        public Note()
        {
            Id = Guid.NewGuid().ToString();
            DateDay = 30;
            DateMonth = 7;
            DateYear = 1983;
            Description = "Birth Date";
        }

        public string Id { get; set; }
        public int DateDay { get; set; }
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public string Description { get; set; }
    }

    public class NoteDto
    {
        public NoteDto(Guid id, string description, DateTime date)
        {
            Id = id;
            Description = description;
            Date = date;
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}