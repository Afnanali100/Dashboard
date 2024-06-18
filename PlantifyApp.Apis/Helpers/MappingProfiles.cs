using AutoMapper;
using Microsoft.Extensions.Hosting;
using PlantifyApp.Apis.Dtos;
using PlantifyApp.Core.Models;
using System.Net;
using System.Xml.Linq;

namespace PlantifyApp.Apis.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {

            CreateMap<Posts, PostDto>().ReverseMap();
            CreateMap<Comments, CommentDto>().ReverseMap();
            CreateMap<Likes, LikeDto>().ReverseMap();

        }
    }
}
