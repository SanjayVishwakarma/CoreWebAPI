using CoreWebAPI.Business.Repositories;
using CoreWebAPI.Business.Services;
using CoreWebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IStoryManagement, StoryManagement>();
builder.Services.AddControllers();
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
// Use global exception handler middleware for non-development environments
app.UseGlobalExceptionHandler();
app.UseCors(options =>
options.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
);
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
