namespace News.Vampire.Models
{
    public class Source
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }

        public string Url { get; set; }
    }
}
