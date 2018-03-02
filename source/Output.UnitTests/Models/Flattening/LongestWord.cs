using System;

namespace Output.UnitTests.Models.Flattening
{
    public class Coniosis
    {
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
    }

    public class Volcano
    {
        public Coniosis Coniosis { get; set; } = new Coniosis();
    }

    public class Silico
    {
        public Volcano Volcano { get; set; } = new Volcano();
    }

    public class Microscopic
    {
        public Silico Silico { get; set; } = new Silico();
    }

    public class Ultra
    {
        public Microscopic Microscopic { get; set; } = new Microscopic();
    }

    public class Pneumono
    {
        public Ultra Ultra { get; set; } = new Ultra();
    }

    public class LongestWord
    {
        public Pneumono Pneumono { get; set; } = new Pneumono();

        public static LongestWord Create()
        {
            var word = new LongestWord();
            word.Pneumono.Ultra.Microscopic.Silico.Volcano.Coniosis.Author = "Everett M. Smith";
            word.Pneumono.Ultra.Microscopic.Silico.Volcano.Coniosis.PublishedDate = new DateTime(1935, 2, 23);
            return word;
        }
    }

    public class LongestWordDto
    {
        public string PneumonoUltraMicroscopicSilicoVolcanoConiosisAuthor { get; set; }
        public DateTime PneumonoUltraMicroscopicSilicoVolcanoConiosisPublishedDate { get; set; }
    }
}