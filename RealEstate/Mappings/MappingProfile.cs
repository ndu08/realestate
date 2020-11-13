using AutoMapper;
using RealEstate.Data;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerViewModel>().ForMember(dest => dest.ServiceType, 
                opt => opt.MapFrom(src => src.Service.Type)).ReverseMap();

            CreateMap<Address, AddressViewModel>().ReverseMap();

            CreateMap<CustomerFile, AttachmentViewModel>().ReverseMap();
        }
    }
}
