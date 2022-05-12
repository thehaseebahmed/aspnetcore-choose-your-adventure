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
using Tha.ChooseYourAdventure.Data;
using Tha.ChooseYourAdventure.Library;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Tha.ChooseYourAdventure.WebAPI.Filters;
using FluentValidation;

namespace Tha.ChooseYourAdventure.WebAPI
{
    public class Startup
    {
        private const string ApiTitle = "Create Your Own Adventure";
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
            services.AddAutoMapper(typeof(Library.DummyClass));
            services.AddApplicationInsightsTelemetry();
            services.AddControllers(ConfigureControllers);
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
                SeedDatabase(app);
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
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public void ConfigureSwaggerGen(SwaggerGenOptions options)
        {
            options.CustomSchemaIds(type => type.ToString());
            options.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiTitle, Version = ApiVersion });
        }

        public void AddMyDependencies(IServiceCollection services)
        {
            services.AddMyFluentValidatonsPipeline();
            services.AddMyHandlers();
            services.AddMyRepositories();
            services.AddMyValidators();

            services.AddScoped<IDbSeeder, DbSeeder>();
            services.AddTransient<IValidatorInterceptor, CustomResponseInterceptor>();
        }

        public void SeedDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbSeeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
                dbSeeder.Seed();
            }
        }
    }
}
