namespace Output.Benchmarking.Models.Intense
{
    public class Tag
    {
        public Tag(string name, TagCategory category)
        {
            Name = name;
            Category = category;
        }

        public string Name { get; set; }
        public TagCategory Category { get; set; }
    }

    public class TagCategory
    {
        public TagCategory(string name, bool isPublic)
        {
            Name = name;
            IsPublic = isPublic;
        }

        public bool IsPublic { get; set; }
        public string Name { get; set; }
    }

    public class TagDto
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryIsPublic { get; set; }
    }
}