using AutoMapper;
using News.Vampire.Service.Models.Dto;

namespace News.Vampire.Service.Models.Mappers
{
    public class ModelMappingProfile: Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<Group, GroupDto>();
        }
    }
}
