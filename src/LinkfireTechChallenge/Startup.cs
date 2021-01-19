using Autofac;
using AutoMapper;
using FluentValidation;
using LinkfireTechChallenge.Core.Behaviors;
using LinkfireTechChallenge.Core.Commands;
using LinkfireTechChallenge.Core.Models.Domain;
using LinkfireTechChallenge.Core.Repositories;
using LinkfireTechChallenge.Core.Services;
using LinkfireTechChallenge.Core.Validators;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SpotifyClient.Core.Config;
using SpotifyClient.Core.DTO;
using SpotifyClient.Core.Utils;
using System;
using System.Collections.Generic;

namespace LinkfireTechChallenge
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSwagger(services);
            ConfigureAutoMapper(services);
            ConfigureEntityServices(services);
            ConfigureEntityRepositories(services);
            ConfigureMediatR(services);
            ConfigureController(services);
            SetupJsonProperties();
        }

        private static void SetupJsonProperties()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        private void ConfigureEntityRepositories(IServiceCollection services)
        {
            services.AddSingleton<IArtistRepository, ArtistRepository>();
            services.AddSingleton<IPlaylistRepository, PlaylistRepository>();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TrackDTO, Track>();
                cfg.CreateMap<ArtistTopTracksDTO, ArtistTopTracks>();
                cfg.CreateMap<IEnumerable<string>, AddTracksToPlaylistDTO>()
                    .ForMember(dest => dest.Uris,
                                opt => opt.MapFrom(src => src));
            });
            services.AddSingleton<IMapper>(c => new Mapper(autoMapperConfig));
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Spotify API V1", Version = "v1" });
            });
        }

        private void ConfigureEntityServices(IServiceCollection services)
        {
            services.AddSingleton<IArtistService, ArtistService>();
            services.AddSingleton<IPlaylistService, PlaylistService>();

            var spotifyClientConfig = new SpotifyClientConfig(_configuration.GetValue<string>("Endpoint"), 
                                                                _configuration.GetValue<string>("AccessToken"));
            services.AddSingleton<ISpotifyClientConfig>(s => spotifyClientConfig);
            services.AddSingleton<ISpotifyClient>(s => new SpotifyClient.SpotifyClient(spotifyClientConfig));
        }

        private static void ConfigureController(IServiceCollection services)
        {
            services.AddControllers();
        }

        private static void ConfigureMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(UpdatePlaylistCommandHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>();
            builder.RegisterType<FluentValidationService>().As<IValidationService>();
            builder.RegisterAssemblyTypes(typeof(BaseValidator<>).Assembly).AsClosedTypesOf(typeof(BaseValidator<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API V1"));
        }
    }
}
