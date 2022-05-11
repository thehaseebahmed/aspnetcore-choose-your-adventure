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

using UserAdventures = Tha.ChooseYourAdventure.Library.Resources.UserAdventures;
using Core = Tha.ChooseYourAdventure.Library.Core;

namespace Tha.ChooseYourAdventure.UnitTests
{
    public class UserAdventuresCreateUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<UserAdventure>> _userAdventuresRepo;

        public UserAdventuresCreateUnitTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserAdventures.Create.Mapper());
            });

            _mapper = mappingConfig.CreateMapper();
            _userAdventuresRepo = new Mock<IRepository<UserAdventure>>();

            _userAdventuresRepo.Setup(r => r.CreateAsync(It.IsAny<UserAdventure>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAdventure()
                {
                    Id = Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"),
                    AdventureId = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                    Status = Data.Enums.UserAdventureStatus.InProgress,
                    UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca")
                });
        }

        [Fact]
        public async Task Handler_NewUserAdventure_ReturnsAdventureId()
        {
            // 1. ARRANGE
            var handler = new Core.Create.Handler<UserAdventures.Create.Command, UserAdventure>(
                _mapper,
                _userAdventuresRepo.Object
                );

            var command = new UserAdventures.Create.Command
            {
                AdventureId = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca")
            };

            // 2. ACT
            var result = await handler.Handle(command, CancellationToken.None);

            // 3. ASSERT
            Assert.Equal(Guid.Parse("901e2eef-b0e3-476e-90fa-d624b310c38c"), result.Id);
        }

        [Fact]
        public async Task Validator_NewUserAdventure_RequestIsValid()
        {
            // 1. ARRANGE
            var validator = new UserAdventures.Create.Validation();
            var command = new UserAdventures.Create.Command
            {
                AdventureId = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                UserId = Guid.Parse("db9ca1b6-6f7c-43e0-98c7-a78ca075e4ca")
            };

            // 2. ACT
            var result = validator.TestValidate(command);

            // 3. ASSERT
            result.ShouldNotHaveValidationErrorFor(r => r.AdventureId);
            result.ShouldNotHaveValidationErrorFor(r => r.UserId);
        }

        [Fact]
        public async Task Validator_NewUserAdventure_RequestIsInValid()
        {
            // 1. ARRANGE
            var validator = new UserAdventures.Create.Validation();
            var command = new UserAdventures.Create.Command { };

            // 2. ACT
            var result = validator.TestValidate(command);

            // 3. ASSERT
            result.ShouldHaveValidationErrorFor(r => r.AdventureId);
            result.ShouldHaveValidationErrorFor(r => r.UserId);
        }
    }
}