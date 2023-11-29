using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Vampire.Service.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(16)]
        public required string Login { get; set; }

        [StringLength(64)]
        public required string Password { get; set; }

        [StringLength(32)]
        public required string Role { get; set; }

        public virtual ICollection<Subscription>? Subscriptions { get; set; }
    }
}
