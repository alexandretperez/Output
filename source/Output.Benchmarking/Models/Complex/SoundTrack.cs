namespace Output.Benchmarking.Models.Complex
{
    public class SoundTrack
    {
        public SoundTrack(string name, int length, Composer composer)
        {
            Name = name;
            Length = length;
            Composer = composer;
        }

        public string Name { get; set; }
        public int Length { get; set; }
        public Composer Composer { get; set; }
    }

    public class SoundTrackDto
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string ComposerName { get; set; }
    }
}