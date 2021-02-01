using AutoMapper;
using Covid19App.Data;
using Covid19App.Models;
using Covid19App.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19App.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientViewModel>()
                .ForMember(dest => dest.PatientGroup,
                opt => opt.MapFrom(src => src.Level.ToString()));

            CreateMap<PatientPostModel, Patient>();

            CreateMap<Patient, PatientAdminViewModel>();

            CreateMap<ApplicationUser, UserModel>();
            CreateMap<ApplicationUser, UserCreateModel>();


        }
    }
}
