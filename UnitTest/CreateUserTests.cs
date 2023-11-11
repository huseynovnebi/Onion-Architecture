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
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Application.Repository;
using Infastructure;
using Application.Models.Validators.User;

namespace UnitTest
{
    public class CreateUserTests
    {
        private readonly UserController _usercon;
        private readonly Unitofwork _unitofwork;
        private readonly IFixture _fixture;
        private readonly Mock<IUnitofwork> _serviceMock;
        private readonly Mock<IMapper> _serviceMock2;
        private readonly Mock<IMemoryCache> _serviceMock3;
        private readonly Mock<UsersDbContext> _serviceMock4;

        public CreateUserTests()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IUnitofwork>>();
            _serviceMock2 = _fixture.Freeze<Mock<IMapper>>();
            _serviceMock3 = new Mock<IMemoryCache>();
            _serviceMock4 = new Mock<UsersDbContext>();
            _usercon = new UserController(_serviceMock.Object, _serviceMock2.Object, _serviceMock3.Object);
            _unitofwork = new Unitofwork(_serviceMock4.Object);
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Create_ValidModel_ReturnsCreatedResult()
        {
            // Arrange
            CreateReqUserDTO dto = new CreateReqUserDTO
            {
                Id = 1,
                Name = "John",
                Age = 10
            };
            User node = _fixture.Create<User>();

            CreateReqUser validator = new CreateReqUser();


            _serviceMock2.Setup(x => x.Map<User>(dto)).Returns(node);
            _serviceMock.Setup(x => x.User.Add(node)).Verifiable();
            _serviceMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            // Action
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new System.Exception("Validation Error");
            }

            var result = await _usercon.Create(dto) as CreatedResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();
            result.StatusCode.Should().Be(201);
            result.Value.Should().Be("Created");
        }

        [Test]
        public async Task Create_ValidModel_SaveChangeFails_ReturnsServerError()
        {
            //Arrange
            var dto = new CreateReqUserDTO
            {
                Id = 1,
                Name = "John",
                Age = 10
            };
            User node = _fixture.Create<User>();
            _serviceMock2.Setup(x => x.Map<User>(dto)).Returns(node);
            _serviceMock.Setup(uow => uow.User.Add(node)); //It.IsAny<User>())
            _serviceMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);

            //Action
            var result = await _usercon.Create(dto) as ObjectResult;

            // Assert
            result.StatusCode.Should().Be(500);

        }

    

    }
}