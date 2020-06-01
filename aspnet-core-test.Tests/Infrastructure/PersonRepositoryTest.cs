using aspnet_core_test.Infrastructure;
using aspnet_core_test.Infrastructure.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xunit;

namespace aspnet_core_test.Tests.Infrastructure
{
    public class PersonRepositoryTest : IDisposable
    {
        private const string ConnectionString = "data source=localhost\\sqlexpress;initial catalog=sampleTestDb;User ID=sa;Password=Pa$$w0rd;MultipleActiveResultSets=True";

        public void Dispose()
        {
            TruncateTestData();
        }

        [Fact]
        public void Get_Returns_Entity()
        {
            // Arrange
            var configMock = CreateMockOfConfiguration();
            var repo = new PersonRepository(configMock.Object);
            var entities = GetPersonEntity();
            InsertTestData(entities);

            // Act
            var result = repo.Get();

            // Assert
            Assert.Equal(entities.Count, result.Count);
            for (int i = 0; i < entities.Count; i++)
            {
                Assert.NotEqual(0, result[i].Id);
                Assert.Contains(entities[i].Code, result[i].Code);
                Assert.Contains(entities[i].Name, result[i].Name);
            }
        }

        [Fact]
        public void GetById_Returns_Entity()
        {
            // Arrange
            var configMock = CreateMockOfConfiguration();
            var repo = new PersonRepository(configMock.Object);
            var entities = GetPersonEntity();
            InsertTestData(entities);
            var target = GetTestData(entities[0].Code);

            // Act
            var result = repo.GetById(target.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(entities[0].Code, result.Code);
            Assert.Contains(entities[0].Name, result.Name);
        }

        [Fact]
        public void Create_Entity()
        {
            // Arrange
            var configMock = CreateMockOfConfiguration();
            var repo = new PersonRepository(configMock.Object);
            var target = new PersonEntity { Code = "01", Name = "Test01" };

            // Act
            var result = repo.Create(target);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Update_Entity()
        {
            // Arrange
            var configMock = CreateMockOfConfiguration();
            var repo = new PersonRepository(configMock.Object);
            var target = new PersonEntity { Code = "01", Name = "Test01" };
            InsertTestData(target);
            var entity = GetTestData(target.Code);
            entity.Name = "Test02";

            // Act
            var result = repo.Update(entity.Id, entity);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Delete_Entity()
        {
            // Arrange
            var configMock = CreateMockOfConfiguration();
            var repo = new PersonRepository(configMock.Object);
            var target = new PersonEntity { Code = "01", Name = "Test01" };
            InsertTestData(target);
            var entity = GetTestData(target.Code);

            // Act
            var result = repo.Delete(entity.Id);

            // Assert
            Assert.Equal(1, result);
        }

        private Mock<IConfiguration> CreateMockOfConfiguration()
        {
            var configSectionMock = new Mock<IConfigurationSection>();
            configSectionMock.SetupGet(m => m[It.Is<string>(s => s == "DefaultConnection")]).Returns(ConnectionString);
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(configSectionMock.Object);
            return configMock;
        }

        private void InsertTestData(List<PersonEntity> entities)
        {
            var query = "INSERT INTO Person (Code, Name) VALUES (@Code, @Name)";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("TRUNCATE TABLE Person");
                foreach (var entity in entities)
                {
                    connection.Execute(query, entity);
                }
            }
        }

        private void InsertTestData(PersonEntity entity)
        {
            var query = "INSERT INTO Person (Code, Name) VALUES (@Code, @Name)";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("TRUNCATE TABLE Person");
                connection.Execute(query, entity);
            }
        }

        private PersonEntity GetTestData(string code)
        {
            var query = "SELECT * FROM Person WHERE Code = @Code";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<PersonEntity>(query, new { Code = code }).FirstOrDefault();
            }
        }

        private void TruncateTestData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute("TRUNCATE TABLE Person");
            }
        }

        private List<PersonEntity> GetPersonEntity()
        {
            return new List<PersonEntity> {
                new PersonEntity{ Code = "01", Name = "Test01" },
                new PersonEntity{ Code = "02", Name = "Test02" },
                new PersonEntity{ Code = "03", Name = "Test03" }
            };
        }
    }
}