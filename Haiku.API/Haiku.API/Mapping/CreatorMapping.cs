using AutoMapper;
using Haiku.API.Dtos;
using Haiku.API.Models;

namespace Haiku.API.Mapping
{
    public class CreatorMapping : Profile
    {
        public CreatorMapping()
        {
            CreateMap<CreatorItem, CreatorDto>();
            CreateMap<CreatorDto, CreatorItem>();
        }
    }
}