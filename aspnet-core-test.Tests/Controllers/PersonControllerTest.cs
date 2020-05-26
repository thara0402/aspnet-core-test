using aspnet_core_test.Controllers;
using aspnet_core_test.Infrastructure;
using aspnet_core_test.Infrastructure.Models;
using aspnet_core_test.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
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
            var entity = GetPersonEntity();
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(repo => repo.Get()).Returns(entity);
            var controller = new PersonController(mockRepo.Object, CreateMapper());

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Person>>(viewResult.ViewData.Model);
            Assert.Equal(entity.Count, model.Count);
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
//            mockRepo.Verify();
        }

        private Person CreateModel()
        {
            return new Person { Id = 0, Code = "00", Name = "零奈" };
        }

        private List<PersonEntity> GetPersonEntity()
        {
            return new List<PersonEntity> {
                new PersonEntity{ Id = 1, Code = "01", Name = "一花" },
                new PersonEntity{ Id = 2, Code = "02", Name = "二乃" },
                new PersonEntity{ Id = 3, Code = "03", Name = "三玖" },
                new PersonEntity{ Id = 4, Code = "04", Name = "四葉" },
                new PersonEntity{ Id = 5, Code = "05", Name = "五月" }
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
