using System.ComponentModel.DataAnnotations;

namespace News.Vampire.Service.Models.Dto
{
    public class UserGroupDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public virtual ICollection<SubscriptionDto>? Subscriptions { get; set; }
    }
}
