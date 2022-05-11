using AutoMapper;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.Repositories;
using Xunit;

using Adventures = Tha.ChooseYourAdventure.Library.Resources.Adventures;
using Core = Tha.ChooseYourAdventure.Library.Core;

namespace Tha.ChooseYourAdventure.UnitTests
{
    public class AdventuresCreateUnitTests
    {
        private readonly Mock<IRepository<AdventureNode>> _adventuresRepo;
        private readonly IMapper _mapper;

        public AdventuresCreateUnitTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Adventures.Create.Mapper());
            });

            _adventuresRepo = new Mock<IRepository<AdventureNode>>();
            _mapper = mappingConfig.CreateMapper();

            _adventuresRepo.Setup(r => r.CreateAsync(It.IsAny<AdventureNode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AdventureNode
                {
                    Id = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                    IsRootNode = true,
                    Name = "Doughnut Decision Helper",
                });
        }

        [Fact]
        public async Task Handler_NewAdventure_ReturnsAdventureId()
        {
            // 1. ARRANGE
            var handler = new Core.Create.Handler<Adventures.Create.Command, AdventureNode>(
                _mapper,
                _adventuresRepo.Object
                );

            var command = new Adventures.Create.Command {
                IsRootNode = true,
                Name = "Doughnut Decision Helper",
            };

            // 2. ACT
            var result = await handler.Handle(command, CancellationToken.None);

            // 3. ASSERT
            Assert.Equal(Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"), result.Id);
        }
    }
}