namespace News.Vampire.Service.Models.Dto
{
    public class GroupDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
