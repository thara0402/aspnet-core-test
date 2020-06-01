using aspnet_core_test.Infrastructure.Models;
using aspnet_core_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_test.Infrastructure
{
    public interface IPersonRepository
    {
        List<PersonEntity> Get();

        PersonEntity GetById(int id);

        int Create(PersonEntity entity);

        int Update(int id, PersonEntity entity);

        int Delete(int id);
    }
}
