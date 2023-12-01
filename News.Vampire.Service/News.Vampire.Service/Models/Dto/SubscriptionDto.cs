namespace News.Vampire.Service.Models.Dto
{
    public class SubscriptionDto
    {
        public int Id { get; set; }

        public virtual SourceDto? Source { get; set; }

        public long? LastLoadedTime { get; set; }
    }
}
