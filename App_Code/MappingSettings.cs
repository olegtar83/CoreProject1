using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using webapp.Models;

namespace webapp
{
    public class MappingSettings:Profile
    {
        public MappingSettings()
        {
            CreateMap<LoginModel, User>().ForMember(d=>d.Name,opt=> { opt.MapFrom(s => s.UserName); });
        }
    }
}
