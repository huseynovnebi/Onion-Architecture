using Api.Controllers;
using NUnit.Framework;
using AutoFixture;
using Moq;
using FluentAssertions;
using Application.Interfaces;
using AutoMapper;
using Application.Models.DTO.User;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest
{
    public class CreateUserTests
    {
        private readonly UserController _usercon;
        private readonly IFixture _fixture;
        private readonly Mock<IUnitofwork> _serviceMock;
        private readonly Mock<IMapper> _serviceMock2;

        public CreateUserTests()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IUnitofwork>>();
            _serviceMock2 = _fixture.Freeze<Mock<IMapper>>();
            _usercon = new UserController(_serviceMock.Object, _serviceMock2.Object);
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Arrange
            CreateReqUserDTO dto = new CreateReqUserDTO
            {
                Id = 1,
                Name = "John",
                Age = "30"
            };
            User node = _serviceMock2.Object.Map<User>(dto);
            _serviceMock.Setup(x => x.User.Add(node));
            _serviceMock.Setup(x => x.SaveChangesAsync());

            //Action
            var evaluator = _usercon.Create(dto).ConfigureAwait(false);
            //Assert
            evaluator.Should().NotBeNull().And.NotBeOfType<OkObjectResult>();
        }
    }
}