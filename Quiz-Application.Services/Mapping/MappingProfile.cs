using AutoMapper;
using DataAccess.Dtos.Concrete;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, Candidate>().ReverseMap();
        }
    }
}
