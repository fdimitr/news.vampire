namespace News.Vampire.Service.Models.Dto
{
    public class SourceDto
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public required string Url { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public ushort Sort { get; set; }

        public long CreatedTime { get; set; }
    }
}
