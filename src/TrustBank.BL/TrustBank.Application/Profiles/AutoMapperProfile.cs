using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Application.Dtos;
using TrustBank.Core.Models;

namespace TrustBank.Application.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile():base()
        {

            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                .ForMember(dest => dest.DateOfLastUpdate, opt => opt.Ignore());

            CreateMap<Customer, CustomerDisplayDto>()
                        .ForMember(dest => dest.AccountDisplayDtos, opt => opt.MapFrom(src => src.Accounts));


            CreateMap<AddressRequestAndDisplayDto, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Customer, opt => opt.Ignore())
                .ForMember(x => x.CustomerId, opt => opt.Ignore())
                .ForMember(x => x.DateCreated, opt => opt.Ignore())
                .ForMember(x => x.DateOfLastUpdate, opt => opt.Ignore());

            CreateMap<Account, AccountDisplayDto>();

            CreateMap<CreateAccountDto, Account>();

            CreateMap<Customer, Account>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForAllOtherMembers(x => x.Ignore());
                
        }
    }
}
