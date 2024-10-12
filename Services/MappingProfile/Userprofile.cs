using AutoMapper;
using Core.Entity;
using Services.Dtos.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public  class Userprofile:Profile
    {
        public Userprofile()
        {
            CreateMap<User, UserProfileDto>()
                .ForMember(d => d.UserId, c => c.MapFrom(s => s.Id))
                .ForMember(d => d.UserName, c => c.MapFrom(s => s.UserName))
                .ForMember(d => d.Email, c => c.MapFrom(s => s.Email))
                .ForMember(d => d.Address, c => c.MapFrom(s => s.Address))
                .ForMember(d => d.wishLists, c => c.MapFrom(s => s.wishLists))
                .ReverseMap();
                
        }
    }
}
