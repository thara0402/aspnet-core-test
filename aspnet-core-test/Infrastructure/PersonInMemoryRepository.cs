using aspnet_core_test.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_test.Infrastructure
{
    public class PersonInMemoryRepository : IPersonRepository
    {
        private List<PersonEntity> _personList;

        public PersonInMemoryRepository()
        {
            _personList = new List<PersonEntity> {
                new PersonEntity{ Id = 1, Code = "01", Name = "一花" },
                new PersonEntity{ Id = 2, Code = "02", Name = "二乃" },
                new PersonEntity{ Id = 3, Code = "03", Name = "三玖" },
                new PersonEntity{ Id = 4, Code = "04", Name = "四葉" },
                new PersonEntity{ Id = 5, Code = "05", Name = "五月" }
            };
        }

        public int Create(PersonEntity entity)
        {
            var maxId = _personList.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
            entity.Id = maxId + 1;
            _personList.Add(entity);
            return 1;
        }

        public int Delete(int id)
        {
            var target = _personList.SingleOrDefault(x => x.Id == id);
            _personList.Remove(target);
            return 1;
        }

        public List<PersonEntity> Get()
        {
            return _personList.OrderBy(x => x.Code).ToList();
        }

        public PersonEntity GetById(int id)
        {
            return _personList.SingleOrDefault(x => x.Id == id);
        }

        public int Update(int id, PersonEntity entity)
        {
            var target = _personList.SingleOrDefault(x => x.Id == id);
            _personList.Remove(target);
            entity.Id = id;
            _personList.Add(entity);
            return 1;
        }
    }
}
