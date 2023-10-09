using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.DTO.User;
using AutoMapper;
using Domain;

namespace Application
{
    public class MapperProfiles :Profile
    {
        public MapperProfiles()
        {
            CreateMap<CreateReqUserDTO, User>().ReverseMap();
            CreateMap<GetReqUserDTO, User>().ReverseMap();
            CreateMap<UpdateUserDTO, User>().ReverseMap();
        }
        
    }
}
