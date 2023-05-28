using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MassTransit;
using System.Reflection;
using ImageService.Models;
using ImageService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix:"HPDS_");

GlobalVariables.serviceAddress = builder.Configuration["HPDS_IMAGE_PATH"];

// Add services to the container.
builder.Services.Configure<ImagesDatabaseSettings>(
    builder.Configuration.GetSection("ImageDatabase"));

builder.Services.AddSingleton<ImagesService>();

//CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(x => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x => {
    x.SetKebabCaseEndpointNameFormatter();
    var entryAssembly = Assembly.GetEntryAssembly();
    x.AddConsumers(entryAssembly);
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration["HPDS_RABBITMQ_HOST"], builder.Configuration["HPDS_RABBITMQ_VHOST"], h => {
            h.Username(builder.Configuration["HPDS_RABBITMQ_USERNAME"]);
            h.Password(builder.Configuration["HPDS_RABBITMQ_PASSWORD"]);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(option=>
{
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "LoginService");
    option.RoutePrefix = string.Empty;
});

app.Run();

public static class GlobalVariables
{
    public static string serviceAddress { get; set; }
}