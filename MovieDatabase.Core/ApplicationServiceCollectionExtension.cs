using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieDatabase.Core.Data;
using MovieDatabase.Core.Repository;
using MovieDatabase.Core.Repository.IRepository;
using MovieDatabase.Core.Services;
using Movies.Application;
using Swashbuckle.AspNetCore.Filters;

namespace MovieDatabase.Core
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPeopleService, PeopleService>();

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPeopleRepository, PeopleRepository>();

            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }

        public static IServiceCollection AddSwaggerExamples(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf<APIResponseBadRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<APIResponseNotFoundExample>();
            services.AddSwaggerExamplesFromAssemblyOf<APIResponseOkExample>();
            services.AddSwaggerExamplesFromAssemblyOf<APIResponseInternalServerErrorExample>();

            return services;
        }
    }
}
