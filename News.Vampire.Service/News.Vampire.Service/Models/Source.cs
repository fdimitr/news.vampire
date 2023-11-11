﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Vampire.Service.Models
{
    public class Source
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long GroupId { get; set; }

        public virtual ICollection<NewsItem>? News { get; set; }

        [StringLength(256)]
        public required string Url { get; set; }

        [StringLength(64)]
        public required string Name { get; set; }

        [StringLength(256)]
        public string? Description { get; set; }

        public ushort Sort { get; set; }

        public ushort UpdateFrequencyMinutes { get; set; }

        public long NextLoadedTime { get; set; }
    }
}
