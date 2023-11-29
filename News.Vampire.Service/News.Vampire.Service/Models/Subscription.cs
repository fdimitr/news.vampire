using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Vampire.Service.Models
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SourceId { get; set; }

        public virtual Source? Source { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public int UserGroupId { get; set; }

        public UserGroup? UserGroup { get; set; }

        public long? LastLoadedTime { get; set; }
    }
}
