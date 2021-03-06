using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using OrdersService.BusinessLogic.Contracts.DomainModels;
using OrdersService.BusinessLogic.Contracts.Persistence;
using OrdersService.WebApi.Managers;
using OrdersService.WebApi.Models;
using Xunit;

namespace OrdersService.Tests.OrdersService.WebApi.Managers
{
    public sealed class OrdersPresenterTests
    {
        public OrdersPresenterTests()
        {
            _ordersRepository = new Mock<IOrdersRepository>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);

            _target = new OrdersPresenter(_ordersRepository.Object, _mapper.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_Order_By_Id()
        {
            var id = _fixture.Create<string>();
            var entity = _fixture.Create<OrderEntity>();
            var model = _fixture.Create<OrderReadModel>();

            _ordersRepository.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(entity));
            _mapper.Setup(x => x.Map<OrderReadModel>(entity)).Returns(model);

            // act
            var actual = await _target.GetByIdAsync(id).ConfigureAwait(false);

            // assert
            _ordersRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
            _mapper.Verify(x => x.Map<OrderReadModel>(entity), Times.Once);
            actual.Should().BeEquivalentTo(model);
        }

        private readonly Mock<IOrdersRepository> _ordersRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly OrdersPresenter _target;
        private readonly Fixture _fixture;
    }
}
