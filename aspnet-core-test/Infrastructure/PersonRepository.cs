using aspnet_core_test.Infrastructure.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_test.Infrastructure
{
    public class PersonRepository : IPersonRepository
    {
        private const string ConnectionStringName = "DefaultConnection";
        private readonly IConfiguration _configuration;

        public PersonRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(PersonEntity entity)
        {
            var query = "INSERT INTO Person (Code, Name) VALUES (@Code, @Name)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return connection.Execute(query, entity);
            }
        }

        public int Delete(int id)
        {
            var query = "DELETE FROM Person WHERE ID = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return connection.Execute(query, new { Id = id });
            }
        }

        public List<PersonEntity> Get()
        {
            var query = "SELECT * FROM Person ORDER BY Code";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return connection.Query<PersonEntity>(query).ToList();
            }
        }

        public PersonEntity GetById(int id)
        {
            var query = "SELECT * FROM Person WHERE ID = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return connection.Query<PersonEntity>(query, new { Id = id }).FirstOrDefault();
            }
        }

        public int Update(int id, PersonEntity entity)
        {
            entity.Id = id;
            var query = "UPDATE Person SET Code = @Code, Name = @Name WHERE ID = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return connection.Execute(query, entity);
            }
        }
    }
}
