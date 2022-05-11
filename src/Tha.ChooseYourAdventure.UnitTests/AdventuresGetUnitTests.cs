using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.Exceptions;
using Tha.ChooseYourAdventure.Library.Repositories;
using Xunit;

using Adventures = Tha.ChooseYourAdventure.Library.Resources.Adventures;

namespace Tha.ChooseYourAdventure.UnitTests
{
    public class AdventuresGetUnitTests
    {
        private readonly Mock<IRepository<AdventureNode>> _adventuresRepo;

        public AdventuresGetUnitTests()
        {
            _adventuresRepo = new Mock<IRepository<AdventureNode>>();

            var adventuresList = new List<AdventureNode>(new[] {
                new AdventureNode()
                {
                    Id = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                    IsRootNode = true,
                    Name = "Doughnut Decision Helper",
                    Children = new[] {
                        new AdventureNode() {
                            Id = Guid.Parse("40700ac8-e618-48c6-bb1f-43dca30b53fe"),
                            Name = "Maybe you want an apple?",
                            OptionTitle = "No"
                        },
                        new AdventureNode() {
                            Id = Guid.Parse("1dde68e6-6ee6-4fa1-a8ea-f6dec530fbad"),
                            Name = "Do I deserve it?",
                            OptionTitle = "Yes",
                            Children = new[] {
                                new AdventureNode() {
                                    Id = Guid.Parse("649c5ea3-34ba-43f3-8fc3-518a097c28d6"),
                                    Name = "Are you sure?",
                                    OptionTitle = "Yes",
                                    Children = new[] {
                                        new AdventureNode() {
                                            Id = Guid.Parse("eeee44f0-ba65-49c2-9230-ace3fe48605a"),
                                            Name = "Get it.",
                                            OptionTitle = "Yes",
                                        },
                                        new AdventureNode() {
                                            Id = Guid.Parse("ef966a4d-22c7-482b-a6de-1248433112d4"),
                                            Name = "Do jumping jacks first.",
                                            OptionTitle = "No"
                                        }
                                    }
                                },
                                new AdventureNode() {
                                    Id = Guid.Parse("e1578374-1414-4dc5-be6f-7af5bd43c5f3"),
                                    Name = "Is it a good doughnut?",
                                    OptionTitle = "No",
                                    Children = new[] {
                                        new AdventureNode() {
                                            Id = Guid.Parse("df5aebb2-8d38-45b7-acde-bd25341a05f1"),
                                            Name = "What are you waiting for? Grab it now.",
                                            OptionTitle = "Yes",
                                        },
                                        new AdventureNode() {
                                            Id = Guid.Parse("e2f40f7a-f87e-45b3-86fb-5e012249748e"),
                                            Name = "Wait til you find a sinful, unforgetable doughnut.",
                                            OptionTitle = "No"
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new AdventureNode()
                {
                    Id = Guid.Parse("07a3df4a-cd28-4b54-a04f-4e00c13bad82"),
                    IsRootNode = true,
                    Name = "Trip to the Bahamas",
                    Children = new[] {
                        new AdventureNode() {
                            Id = Guid.Parse("40700ac8-e618-48c6-bb1f-b53fe43dca30"),
                            Name = "Maybe you don't want a vacation.",
                            OptionTitle = "No"
                        },
                        new AdventureNode() {
                            Id = Guid.Parse("40700ac8-e618-48c6-bb1f-43d3feca30b5"),
                            Name = "Let's go!",
                            OptionTitle = "Yes"
                        }
                    }
                }
            });
            var adventuresListMock = adventuresList.AsQueryable().BuildMock();
            _adventuresRepo.Setup(r => r.Read()).Returns(adventuresListMock);
        }

        [Fact]
        public async Task Handler_GetAllAdventures_ReturnsTwoAdventures()
        {
            // 1. ARRANGE
            var handler = new Adventures.Get.Handler(_adventuresRepo.Object);
            var query = new Adventures.Get.Request { };

            // 2. ACT
            var result = await handler.Handle(query, CancellationToken.None);

            // 3. ASSERT
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task Handler_GetAPageOfAdventures_ReturnsOneAdventurePerPage()
        {
            // 1. ARRANGE
            var handler = new Adventures.Get.Handler(_adventuresRepo.Object);
            var query1 = new Adventures.Get.Request { Limit = 1 };
            var query2 = new Adventures.Get.Request { Limit = 1, Skip = 1 };

            // 2. ACT
            var result1 = await handler.Handle(query1, CancellationToken.None);
            var result2 = await handler.Handle(query2, CancellationToken.None);

            // 3. ASSERT
            Assert.Single(result1.Data);
            Assert.Single(result2.Data);

            Assert.Equal("Doughnut Decision Helper", result1.Data.First().Name);
            Assert.Equal("Trip to the Bahamas", result2.Data.First().Name);
        }

        [Fact]
        public async Task Handler_GetCountOfAdventures_ReturnsCountOfTwo()
        {
            // 1. ARRANGE
            var handler = new Adventures.Get.Handler(_adventuresRepo.Object);
            var query = new Adventures.Get.Request { Count = true };

            // 2. ACT
            var result = await handler.Handle(query, CancellationToken.None);

            // 3. ASSERT
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Handler_GetAdventureById_ReturnsAdventure()
        {
            // 1. ARRANGE
            var handler = new Adventures.GetById.Handler(_adventuresRepo.Object);
            var query = new Adventures.GetById.Request { Id = Guid.Parse("07a3df4a-cd28-4b54-a04f-4e00c13bad82") };

            // 2. ACT
            var result = await handler.Handle(query, CancellationToken.None);

            // 3. ASSERT
            Assert.Equal("Trip to the Bahamas", result.Name);
            Assert.Equal(2, result.Children.Count());
        }

        [Fact]
        public async Task Handler_GetAdventureById_ThrowsNotFoundException()
        {
            // 1. ARRANGE
            var handler = new Adventures.GetById.Handler(_adventuresRepo.Object);
            var query = new Adventures.GetById.Request { Id = Guid.Parse("07a3df4a-0000-0000-0000-4e00c13bad82") };

            // 2/3. ACT & ASSERT
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }
    }
}