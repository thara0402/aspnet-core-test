using aspnet_core_test.Infrastructure.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_test.Models
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<PersonEntity, Person>();
            CreateMap<Person, PersonEntity>();
        }
    }
}
