using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix:"HPDS_");

//CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy( policy =>
        {
            policy.WithOrigins(builder.Configuration["HPDS_FRONTEND_URL"],"http://localhost:3000","https://localhost")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database
builder.Services.AddDbContext<UserDb>(opt=>opt.UseNpgsql(builder.Configuration["HPDS_DB_CONN_STRING"]));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

app.MapGet("/users/{userName}", async (string userName, UserDb db) =>
    await db.Users.SingleOrDefaultAsync(x => x.Username == userName)
        is User user ? Results.Ok(new UserDTO(user)) : Results.NotFound())
.WithName("GetUser");

app.MapPost("/users", async (PostUserDTO userDTO, UserDb db) =>
{
    var user = new User{
        Username = userDTO.Username,
        Email =userDTO.Email,
        Gender = userDTO.Gender
    };
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Username}", new UserDTO(user));
})
.WithName("PostUser");

app.MapDelete("/users/{userName}", async (string userName, UserDb db) =>
{
    if (await db.Users.SingleOrDefaultAsync(x => x.Username == userName) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.Ok(new UserDTO(user));
    }
    return Results.NotFound();
})
.WithName("DeleteUser");

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

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
}

public class UserDTO{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }

    public UserDTO(){ }
    public UserDTO(User user) =>
    (Id, Username, Email, Gender) = (user.Id, user.Username, user.Email, user.Gender);
}

public class PostUserDTO{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }

    public PostUserDTO(){ }
    public PostUserDTO(User user) =>
    (Username, Email, Gender) = (user.Username, user.Email, user.Gender);
}

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
