using AutoMapper;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.Repositories;
using Xunit;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using System.Linq;
using MockQueryable.Moq;

using UserAdventures = Tha.ChooseYourAdventure.Library.Resources.UserAdventures;

namespace Tha.ChooseYourAdventure.UnitTests
{
    public class UserAdventuresPutUnitTests
    {
        private readonly Mock<IRepository<AdventureNode>> _adventuresRepo;
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<UserAdventure>> _userAdventuresRepo;
        private readonly Mock<IRepository<UserAdventureStep>> _userAdventureStepsRepo;

        public UserAdventuresPutUnitTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserAdventures.Put.Mapper());
            });

            _adventuresRepo = new Mock<IRepository<AdventureNode>>();
            _mapper = mappingConfig.CreateMapper();
            _userAdventuresRepo = new Mock<IRepository<UserAdventure>>();
            _userAdventureStepsRepo = new Mock<IRepository<UserAdventureStep>>();

            var adventuresList = new List<AdventureNode>(new[] {
                new AdventureNode()
                {
                    Id = Guid.Parse("07a3df4a-cd28-4b54-a04f-4e00c13bad82"),
                    IsRootNode = true,
                    Name = "Trip to the Bahamas"
                },
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
            });
            var adventuresListMock = adventuresList.AsQueryable().BuildMock();
            _adventuresRepo.Setup(r => r.Read()).Returns(adventuresListMock);

            var userAdventuresList = new List<UserAdventure>(new[] {
                new UserAdventure() {
                    Id = Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"),
                    AdventureId = Guid.Parse("07a3df4a-cd28-4b54-a04f-4e00c13bad82"),
                    Status = Data.Enums.UserAdventureStatus.InProgress,
                    UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca"),
                    Adventure = new AdventureNode()
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
                },
                new UserAdventure() {
                    Id = Guid.Parse("e746139f-9d70-41c8-892f-bad2f08c652e"),
                    AdventureId = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                    Status = Data.Enums.UserAdventureStatus.Completed,
                    UserId = Guid.Parse("8fbbf498-f933-4707-ae97-8d87c2488f0b"),
                    Adventure = new AdventureNode()
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
                                OptionTitle = "Yes"
                            }
                        }
                    },
                    Steps = new []
                    {
                        new UserAdventureStep()
                        {
                            Id = Guid.Parse("76f511a3-52c2-4abe-9f3f-f047eeca95cb"),
                            AdventureStepId = Guid.Parse("1dde68e6-6ee6-4fa1-a8ea-f6dec530fbad"),
                            UserAdventureId = Guid.Parse("e746139f-9d70-41c8-892f-bad2f08c652e"),
                            UserId = Guid.Parse("8fbbf498-f933-4707-ae97-8d87c2488f0b"),
                            AdventureStep = new AdventureNode() {
                                Id = Guid.Parse("1dde68e6-6ee6-4fa1-a8ea-f6dec530fbad"),
                                Name = "Do I deserve it?",
                                OptionTitle = "Yes",
                                Children = new[] {
                                    new AdventureNode() {
                                        Id = Guid.Parse("649c5ea3-34ba-43f3-8fc3-518a097c28d6"),
                                        Name = "Are you sure?",
                                        OptionTitle = "Yes"
                                    },
                                    new AdventureNode() {
                                        Id = Guid.Parse("e1578374-1414-4dc5-be6f-7af5bd43c5f3"),
                                        Name = "Is it a good doughnut?",
                                        OptionTitle = "No"
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var userAdventuresListMock = userAdventuresList.AsQueryable().BuildMock();
            _userAdventuresRepo.Setup(r => r.Read()).Returns(userAdventuresListMock);

            _userAdventureStepsRepo.Setup(r => r.CreateAsync(It.IsAny<UserAdventureStep>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAdventureStep()
                {
                    Id = Guid.Parse("7c45e0a6-2241-4c0b-984d-ba55dfead743"),
                    AdventureStepId = Guid.Parse("40700ac8-e618-48c6-bb1f-b53fe43dca30"),
                    UserAdventureId = Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"),
                    UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca"),
                    AdventureStep = new AdventureNode()
                    {
                        Id = Guid.Parse("40700ac8-e618-48c6-bb1f-b53fe43dca30"),
                        Name = "Maybe you don't want a vacation.",
                        OptionTitle = "No"
                    }
                });
        }

        [Fact]
        public async Task Handler_NewUserAdventureStep_ReturnsUserAdventureStepId()
        {
            // 1. ARRANGE
            var handler = new UserAdventures.Put.Handler(
                _adventuresRepo.Object,
                _mapper,
                _userAdventuresRepo.Object,
                _userAdventureStepsRepo.Object
                );

            var command = new UserAdventures.Put.Command
            {
                AdventureStepId = Guid.Parse("40700ac8-e618-48c6-bb1f-b53fe43dca30"),
                UserAdventureId = Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"),
                UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca")
            };

            // 2. ACT
            var result = await handler.Handle(command, CancellationToken.None);

            // 3. ASSERT
            Assert.Equal(Guid.Parse("7c45e0a6-2241-4c0b-984d-ba55dfead743"), result.Id);
        }

        [Fact]
        public async Task Validator_NewUserAdventureStep_RequestIsValid()
        {
            // 1. ARRANGE
            var validator = new UserAdventures.Put.Validation(_userAdventuresRepo.Object);
            var command = new UserAdventures.Put.Command
            {
                AdventureStepId = Guid.Parse("40700ac8-e618-48c6-bb1f-b53fe43dca30"),
                UserAdventureId = Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"),
                UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca")
            };

            // 2. ACT
            var result = validator.TestValidate(command);

            // 3. ASSERT
            result.ShouldNotHaveValidationErrorFor(r => r);
        }

        [Fact]
        public async Task Validator_NewUserAdventureStep_NotAValidNextStep()
        {
            // 1. ARRANGE
            var validator = new UserAdventures.Put.Validation(_userAdventuresRepo.Object);
            var command = new UserAdventures.Put.Command
            {
                AdventureStepId = Guid.Parse("1dde68e6-6ee6-4fa1-a8ea-f6dec530fbad"),
                UserAdventureId = Guid.Parse("e746139f-9d70-41c8-892f-bad2f08c652e"),
                UserId = Guid.Parse("8fbbf498-f933-4707-ae97-8d87c2488f0b")
            };

            // 2. ACT
            var result = validator.TestValidate(command);

            // 3. ASSERT
            result.ShouldHaveValidationErrorFor(r => r);
        }
    }
}