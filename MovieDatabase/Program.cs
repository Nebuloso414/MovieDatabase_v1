using Microsoft.EntityFrameworkCore;
using MovieDatabase;
using MovieDatabase.Core.Data;
using MovieDatabase.Core.Repository;
using MovieDatabase.Core.Repository.IRepository;
using MovieDatabase.Core.Services;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDB")));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ExampleFilters();
});

// Register Swagger examples
builder.Services.AddSwaggerExamplesFromAssemblyOf<APIResponseBadRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<APIResponseNotFoundExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<APIResponseOkExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<APIResponseInternalServerErrorExample>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
