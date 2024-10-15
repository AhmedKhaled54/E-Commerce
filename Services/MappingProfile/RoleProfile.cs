using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Services.Dtos.RoleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleDetailsDto>()
               .ForMember(d => d.RoleId, c => c.MapFrom(s=>s.Id))
               .ForMember(d=>d.RoleName,c=>c.MapFrom(s=>s.Name))
               .ReverseMap();
        }
    }
}
