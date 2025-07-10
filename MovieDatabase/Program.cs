using Microsoft.EntityFrameworkCore;
using MovieDatabase;
using MovieDatabase.Api;
using MovieDatabase.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("MovieDB")!);
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services
    .AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Register Swagger examples
builder.Services.AddSwaggerExamples();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationMiddleware>();
app.MapControllers();

app.Run();
