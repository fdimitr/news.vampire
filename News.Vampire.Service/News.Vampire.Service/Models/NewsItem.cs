using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Vampire.Service.Models
{
    public class NewsItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int SourceId { get; set; }

        public Source? Source { get; set; }

        [StringLength(512)]
        public required string Title { get; set; }

        [StringLength(1024)]
        [Column(TypeName = "jsonb")]
        public List<string>? Url { get; set; }

        [StringLength(1024)]
        public string? Description { get; set; }

        [StringLength(512)]
        [Column(TypeName = "jsonb")]
        public List<string>? Category { get; set; }

        public long PublicationDate { get; set; }

        public long TimeStamp { get; set; }

        public string? ExternalId { get; set; }

        [StringLength(256)]
        [Column(TypeName = "jsonb")]
        public List<string>? Author { get; set; }

        public string? ImageUrl { get; set; }
    }
}
