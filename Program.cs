using FoodRatingApi.Entities;
using FoodRatingApi.Extensions;
using FoodRatingApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;
builder.Services.AddDbContext<FoodRatingDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("Default"));
});

builder.Services.AddMemoryCache();
builder.Services.AddObjectStorage();
builder.Services.AddScoped<IFoodRatingDtoMapper, FoodRatingDtoMapper>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = config.GetValue<string>("Auth0:Authority");
    options.Audience = config.GetValue<string>("Auth0:Audience");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Automatically migrate database on startup.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FoodRatingDbContext>();
    context.Database.Migrate();
}

app.Run();
