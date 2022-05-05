using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tha.ChooseYourAdventure.Data.Entities;
using System;
using Tha.ChooseYourAdventure.Library.ViewModels;

namespace Tha.ChooseYourAdventure.Library
{
    public static class IServicesCollectionExtensions
    {
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
            #region WeatherForecast Handlers

            services.AddScoped(
                typeof(IRequestHandler<Core.Get.Request<WeatherForecast>, PagedResultViewModel<WeatherForecast>>),
                typeof(Core.Get.Handler<WeatherForecast>)
                );

            services.AddScoped(
                typeof(IRequestHandler<Resources.WeatherForecasts.Create.Command, CommandResultViewModel>),
                typeof(Core.Create.Handler<Resources.WeatherForecasts.Create.Command, WeatherForecast, Guid>)
                );

            #endregion

            return services;
        }
    }
}
