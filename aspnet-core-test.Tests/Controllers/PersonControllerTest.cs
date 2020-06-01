using aspnet_core_test.Controllers;
using aspnet_core_test.Infrastructure;
using aspnet_core_test.Infrastructure.Models;
using aspnet_core_test.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Xunit;

namespace aspnet_core_test.Tests.Controllers
{
    public class PersonControllerTest
    {
        [Fact]
        public void Index_Returns_ViewResult()
        {
            // Arrange
            var entities = GetPersonEntities();
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(repo => repo.Get()).Returns(entities);
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Person>>(viewResult.ViewData.Model);
            Assert.Equal(entities.Count, model.Count);
        }

        [Fact]
        public void Details_Returns_ViewResult()
        {
            // Arrange
            var entity = GetPersonEntity();
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(repo => repo.GetById(entity.Id)).Returns(entity);
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Details(entity.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.ViewData.Model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Code, model.Code);
            Assert.Equal(entity.Name, model.Name);
        }

        [Fact]
        public void Details_Returns_NotFoundResult()
        {
            // Arrange
            var entity = GetPersonEntity();
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(repo => repo.GetById(entity.Id)).Returns(entity);
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Details(2);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Returns_ViewResult()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreatePost_Returns_RedirectToActionResult()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Create(CreateModel());

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public void CreatePost_Returns_BadRequestResult()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();
            var controller = new PersonController(mockRepo.Object, CreateMapper());
            controller.ModelState.AddModelError("Code", "Required");

            // Act
            var result = controller.Create(CreateModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Edit_Returns_ViewResult()
        {
            // Arrange
            var entity = GetPersonEntity();
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(repo => repo.GetById(entity.Id)).Returns(entity);
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Edit(entity.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.ViewData.Model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Code, model.Code);
            Assert.Equal(entity.Name, model.Name);
        }

        [Fact]
        public void Edit_Returns_NotFoundResult()
        {
            // Arrange
            var entity = GetPersonEntity();
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(repo => repo.GetById(entity.Id)).Returns(entity);
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Edit(2);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void EditPost_Returns_RedirectToActionResult()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();
            var controller = new PersonController(mockRepo.Object, CreateMapper());
            var model = CreateModel();

            // Act
            var result = controller.Edit(model.Id, model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public void EditPost_Returns_BadRequestResult()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();
            var controller = new PersonController(mockRepo.Object, CreateMapper());
            controller.ModelState.AddModelError("Code", "Required");
            var model = CreateModel();

            // Act
            var result = controller.Edit(model.Id, model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Delete_Returns_ViewResult()
        {
            // Arrange
            var entity = GetPersonEntity();
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(repo => repo.GetById(entity.Id)).Returns(entity);
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Delete(entity.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.ViewData.Model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Code, model.Code);
            Assert.Equal(entity.Name, model.Name);
        }

        [Fact]
        public void DeleteConfirmed_Returns_RedirectToActionResult()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        private Person CreateModel()
        {
            return new Person { Id = 1, Code = "01", Name = "Test01" };
        }

        private PersonEntity GetPersonEntity()
        {
            return new PersonEntity{ Id = 1, Code = "01", Name = "Test01" };
        }

        private List<PersonEntity> GetPersonEntities()
        {
            return new List<PersonEntity> {
                new PersonEntity{ Id = 1, Code = "01", Name = "Test01" },
                new PersonEntity{ Id = 2, Code = "02", Name = "Test02" },
                new PersonEntity{ Id = 3, Code = "03", Name = "Test03" }
            };
        }

        private IMapper CreateMapper()
        {
            var mockMapper = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapping>();
            });
            return mockMapper.CreateMapper();
        }
    }
}
