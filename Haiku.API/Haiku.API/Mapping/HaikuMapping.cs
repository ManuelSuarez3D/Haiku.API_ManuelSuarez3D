using AutoMapper;
using Haiku.API.Dtos;
using Haiku.API.Models;

namespace Haiku.API.Mapping
{
    public class HaikuMapping : Profile
    {
        public HaikuMapping()
        {
            CreateMap<HaikuItem, HaikuDto>();
            CreateMap<HaikuDto, HaikuItem>();
        }
    }
}
