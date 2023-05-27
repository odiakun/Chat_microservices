using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using LoginService.Models;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix:"HPDS_");

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

//Database
// builder.Services.AddDbContext<UserDb>(opt=>opt.UseNpgsql(builder.Configuration["HPDS_DB_CONN_STRING"]), ServiceLifetime.Scoped);
builder.Services.AddDbContext<UserDb>(opt=>opt.UseNpgsql(builder.Configuration["HPDS_DB_CONN_STRING"]));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UserDb>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}


app.Run();

public class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options) 
    : base(options) { }
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasAlternateKey(x => x.Username);
    }
}
