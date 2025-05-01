using Moq;
using Signature.API.Application.Services;
using Signature.API.Application.DTOs;    
using Signature.API.Domain.Entities;
using Signature.API.Domain.Interfaces;

namespace Signature.Tests.UnitTests
{
    public class SubscriptionPlanServiceTests
    {
        private readonly Mock<ISubscriptionPlanRepository> _repoMock;
        private readonly SubscriptionPlanService _service;

        public SubscriptionPlanServiceTests()
        {
            _repoMock = new Mock<ISubscriptionPlanRepository>();
            _service = new SubscriptionPlanService(_repoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_DeveAdicionarPlano_E_RetornarDto()   // 11
        {
            var dto = new CreateSubscriptionPlanDto("Plano Ouro", 99.90m);
            SubscriptionPlan capturedEntity = null!;

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<SubscriptionPlan>()))
                .Callback<SubscriptionPlan>(entity => capturedEntity = entity)
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<SubscriptionPlan>()), Times.Once);

            Assert.Equal(capturedEntity.Id, result.Id);
            Assert.Equal("Plano Ouro", result.Title);
            Assert.Equal(99.90m, result.Price);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaDeDtos()           // 15
        {
            var plans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan("Bronze", 29.90m),
                new SubscriptionPlan("Prata", 59.90m)
            };
            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(plans);

            var result = (await _service.GetAllAsync()).ToList();

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Title == "Bronze" && p.Price == 29.90m);
            Assert.Contains(result, p => p.Title == "Prata" && p.Price == 59.90m);
        }

        [Fact]
        public async Task GetByIdAsync_QuandoExiste_DeveRetornarDto()
        {
            var id = Guid.NewGuid();
            var plan = new SubscriptionPlan("Diamante", 199.90m);
            typeof(SubscriptionPlan)
                .GetProperty("Id")!
                .SetValue(plan, id);

            _repoMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(plan);

            var result = await _service.GetByIdAsync(id);

            _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
            Assert.Equal("Diamante", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            var id = Guid.NewGuid();
            _repoMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((SubscriptionPlan?)null);

            var result = await _service.GetByIdAsync(id);

            _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
            Assert.Null(result);
        }
    }
}
