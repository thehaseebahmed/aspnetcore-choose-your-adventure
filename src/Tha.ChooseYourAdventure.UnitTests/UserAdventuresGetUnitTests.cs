using AutoMapper;
using FluentValidation.TestHelper;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Entities;
using Tha.ChooseYourAdventure.Library.Repositories;
using Xunit;

using UserAdventures = Tha.ChooseYourAdventure.Library.Resources.UserAdventures;

namespace Tha.ChooseYourAdventure.UnitTests
{
    public class UserAdventuresGetUnitTests
    {
        private readonly Mock<IRepository<UserAdventure>> _userAdventuresRepo;
        private readonly IMapper _mapper;

        public UserAdventuresGetUnitTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserAdventures.Get.Mapper());
            });

            _userAdventuresRepo = new Mock<IRepository<UserAdventure>>();
            _mapper = mappingConfig.CreateMapper();

            var userAdventuresList = new List<UserAdventure>(new[] {
                new UserAdventure() {
                    Id = Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"),
                    AdventureId = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                    Status = Data.Enums.UserAdventureStatus.Completed,
                    UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca"),
                    Adventure = new AdventureNode()
                    {
                        Id = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                        IsRootNode = true,
                        Name = "Doughnut Decision Helper"
                    },
                    Steps = new []
                    {
                        new UserAdventureStep()
                        {
                            Id = Guid.Parse("7c45e0a6-2241-4c0b-984d-ba55dfead743"),
                            AdventureStepId = Guid.Parse("40700ac8-e618-48c6-bb1f-43dca30b53fe"),
                            UserAdventureId = Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"),
                            UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca"),
                            AdventureStep = new AdventureNode() {
                                Id = Guid.Parse("40700ac8-e618-48c6-bb1f-43dca30b53fe"),
                                Name = "Maybe you want an apple?",
                                OptionTitle = "No"
                            }
                        }
                    }
                }
            });
            var userAdventuresListMock = userAdventuresList.AsQueryable().BuildMock();
            _userAdventuresRepo.Setup(r => r.Read()).Returns(userAdventuresListMock);
        }

        [Fact]
        public async Task Handler_GetAllUserAdventures_ReturnsOneUserAdventure()
        {
            // 1. ARRANGE
            var handler = new UserAdventures.Get.Handler(_userAdventuresRepo.Object);
            var query = new UserAdventures.Get.Request { UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca") };

            // 2. ACT
            var result = await handler.Handle(query, CancellationToken.None);

            // 3. ASSERT
            Assert.Single(result.Data);
            Assert.Equal(Data.Enums.UserAdventureStatus.Completed, result.Data.ElementAt(0).Status);
        }

        [Fact]
        public async Task Validator_GetAllUserAdventures_IsValid()
        {
            // 1. ARRANGE
            var validator = new UserAdventures.Get.Validation();
            var query = new UserAdventures.Get.Request { UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca") };

            // 2. ACT
            var result = validator.TestValidate(query);

            // 3. ASSERT
            result.ShouldNotHaveValidationErrorFor(r => r.UserId);
        }

        [Fact]
        public async Task Validator_GetAllUserAdventures_HasInValidUserId()
        {
            // 1. ARRANGE
            var validator = new UserAdventures.Get.Validation();
            var query = new UserAdventures.Get.Request { };

            // 2. ACT
            var result = validator.TestValidate(query);

            // 3. ASSERT
            result.ShouldHaveValidationErrorFor(r => r.UserId);
        }
    }
}