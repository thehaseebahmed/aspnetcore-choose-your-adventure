using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Starter.Data;
using Starter.Library;
using Starter.WebAPI.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Starter.WebAPI
{
    public class Startup
    {
        private const string ApiTitle = "ASP.NET Core WebAPI Starter";
        private const string ApiVersion = "v1";
        private const string ApiDocsRoutePrefix = "";

        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(ConfigureControllers)
                    .AddFluentValidation(ConfigureFluentValidation);

            services.AddAutoMapper(typeof(Library.DummyClass));
            services.AddApplicationInsightsTelemetry();
            services.AddDbContext<ApiDbContext>(ConfigureDbContext);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSwaggerGen(ConfigureSwaggerGen);

            AddMyDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", $"{ApiTitle} {ApiVersion}");
                c.RoutePrefix = ApiDocsRoutePrefix;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public void ConfigureControllers(MvcOptions options)
        {
            options.Filters.Add(typeof(GlobalExceptionFilter));
        }

        public void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("InMemory Database");
        }

        public void ConfigureFluentValidation(FluentValidationMvcConfiguration options)
        {
            options.ImplicitlyValidateChildProperties = true;
            options.RegisterValidatorsFromAssemblyContaining<Library.DummyClass>();
            options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
        }

        public void ConfigureSwaggerGen(SwaggerGenOptions options)
        {
            options.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiTitle, Version = ApiVersion });
        }

        public void AddMyDependencies(IServiceCollection services)
        {
            services.AddMyHandlers();
            services.AddTransient<IValidatorInterceptor, CustomResponseInterceptor>();
        }

        public void SeedDatabase(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<ApiDbContext>();
            Seeder.Seed(context);
        }
    }
}
