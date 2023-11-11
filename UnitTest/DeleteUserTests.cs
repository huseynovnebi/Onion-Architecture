using Api.Controllers;
using Application.Interfaces;
using NUnit.Framework;
using AutoFixture;
using Moq;
using FluentAssertions;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Infastructure;
using Application.Repository;
using Domain;

namespace UnitTest
{
    public class DeleteUserTests
    {
        private readonly UserController _usercon;
        private readonly Unitofwork _unitofwork;
        private readonly IFixture _fixture;
        private readonly Mock<IUnitofwork> _serviceMock;
        private readonly Mock<IMapper> _serviceMock2;
        private readonly Mock<IMemoryCache> _serviceMock3;
        private readonly Mock<UsersDbContext> _serviceMock4;


        public DeleteUserTests()
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
        public async Task RequestId_Should_Notbe_LessAndEqualTo_Zero()
        {
            //Arrange
            int id = 0;


            //Action
            var evaluator = await _usercon.Delete(id).ConfigureAwait(false);

            //Assert
            evaluator.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();

        }

        [Test]
        public async Task Delete_ValidModel_ReturnsDeleteResult()
        {
            //Arrange
            int id = 25;
            _serviceMock.Setup(x => x.User.GetByIdStrictAsync(id)).ReturnsAsync(new User { Id = 25, Name = "nebi", Age = 25 });
            _serviceMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            //Action
            var evaluator = await _usercon.Delete(id) as ObjectResult;

            //Assert
            evaluator.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        }

        [Test]
        public async Task Delete_UserNotFound_ReturnsNotFound()
        { 
            //Arrange
            int id = 25;
            User? node =null;

            _serviceMock.Setup(x => x.User.GetByIdStrictAsync(id)).ReturnsAsync(node);
            _serviceMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            //Action
            var evaluator = await _usercon.Delete(id) as ObjectResult;

            //Assert
            evaluator.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();

        }
    }
}