using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tha.ChooseYourAdventure.Library.ViewModels;
using System;
using Adventures = Tha.ChooseYourAdventure.Library.Resources.Adventures;
using UserAdventures = Tha.ChooseYourAdventure.Library.Resources.UserAdventures;
using Tha.ChooseYourAdventure.Library.Repositories;
using FluentValidation;
using Tha.ChooseYourAdventure.Library.PipelineBehaviors;

namespace Tha.ChooseYourAdventure.Library
{
    public static class IServicesCollectionExtensions
    {
        public static IServiceCollection AddMyFluentValidatonsPipeline(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            return services;
        }

        public static IServiceCollection AddMyHandlers(this IServiceCollection services)
        {
            #region Sample Handlers

            // EXAMPLE OF A GET HANDLER
            // services.AddScoped(
            //     typeof(IRequestHandler<Library.Core.Get.Request<T>, PagedResultViewModel<T>>),
            //     typeof(Library.Core.Get.Handler<T>)
            //     );

            // EXAMPLE OF A CREATE HANDLER
            // services.AddScoped(
            //     typeof(IRequestHandler<Library.Resources.Something.Create.Command, CommandResultViewModel>),
            //     typeof(Library.Core.Create.Handler<Library.Resources.Something.Create.Command, Models.Entities.Something, TKey>)
            //     );

            #endregion
            #region Adventures Handlers

            services.AddScoped(
                typeof(IRequestHandler<Adventures.Get.Request, PagedResultViewModel<Data.Entities.AdventureNode>>),
                typeof(Adventures.Get.Handler)
                );

            services.AddScoped(
                typeof(IRequestHandler<Adventures.GetById.Request, Data.Entities.AdventureNode>),
                typeof(Adventures.GetById.Handler)
                );

            services.AddScoped(
                typeof(IRequestHandler<Adventures.Create.Command, CommandResultViewModel>),
                typeof(Core.Create.Handler<Adventures.Create.Command, Data.Entities.AdventureNode>)
                );

            #endregion
            #region User Adventures Handlers

            services.AddScoped(
                typeof(IRequestHandler<UserAdventures.Get.Request, PagedResultViewModel<Data.Entities.UserAdventure>>),
                typeof(UserAdventures.Get.Handler)
                );

            services.AddScoped(
                typeof(IRequestHandler<Core.GetById.Request<Data.Entities.AdventureNode>, Data.Entities.AdventureNode>),
                typeof(Core.GetById.Handler<Data.Entities.AdventureNode>)
                );

            services.AddScoped(
                typeof(IRequestHandler<UserAdventures.Create.Command, CommandResultViewModel>),
                typeof(Core.Create.Handler<UserAdventures.Create.Command, Data.Entities.UserAdventure>)
                );

            services.AddScoped(
                typeof(IRequestHandler<UserAdventures.Put.Command, CommandResultViewModel>),
                typeof(UserAdventures.Put.Handler)
                );

            #endregion

            return services;
        }

        public static IServiceCollection AddMyRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Data.Entities.AdventureNode>), typeof(EFCoreRepository<Data.Entities.AdventureNode>));
            services.AddScoped(typeof(IRepository<Data.Entities.UserAdventure>), typeof(EFCoreRepository<Data.Entities.UserAdventure>));
            services.AddScoped(typeof(IRepository<Data.Entities.UserAdventureStep>), typeof(EFCoreRepository<Data.Entities.UserAdventureStep>));

            return services;
        }

        public static IServiceCollection AddMyValidators(this IServiceCollection services)
        {
            AssemblyScanner
                .FindValidatorsInAssembly(typeof(Library.DummyClass).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

            return services;
        }
    }
}
