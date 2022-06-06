using Microsoft.EntityFrameworkCore;
using PocApi.Models;
using PocApi.Services;
using PocApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ComicStoreDatabaseSettings>(
    builder.Configuration.GetSection("ComicStoreDatabase"));

// Add the comic service.
builder.Services.AddScoped<IComicService, ComicsService>();
builder.Services.AddScoped<IIssueService, IssueService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "TodoApi", Version = "v1" });
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
