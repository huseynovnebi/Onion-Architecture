using Api.Controllers;
using Application.Interfaces;
using NUnit.Framework;
using AutoFixture;
using Moq;
using FluentAssertions;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest
{
    public class DeleteUserTests
    {
        private readonly UserController _usercon;
        private readonly IFixture _fixture;
        private readonly Mock<IUnitofwork> _serviceMock;
        private readonly Mock<IMapper> _serviceMock2;


        public DeleteUserTests()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IUnitofwork>>();
            _serviceMock2 = _fixture.Freeze<Mock<IMapper>>();
            //_usercon = new UserController(_serviceMock.Object,_serviceMock2.Object);
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
            evaluator.Should().NotBeNull().And.NotBeOfType<BadRequestObjectResult>();

        }

        [Test]
        public async Task IsSaveChanged()
        {
            //Arrange
            int id = 2;

            //Action
            var evaluator = await _usercon.Delete(id).ConfigureAwait(false);

            //Assert
            evaluator.Should().NotBeNull().And.NotBeOfType<BadRequestObjectResult>();

        }
    }
}
