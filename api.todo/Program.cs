using api.todo.Context;
using api.todo.Services;
using api.todo.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using api.todo.Repository;
using api.todo.Repository.Impl;
using StackExchange.Redis;
using api.todo.Utils;

var builder = WebApplication.CreateBuilder(args);

// Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
         ClockSkew = TimeSpan.Zero
     };
 });
// Jwt configuration ends here

// Add repository to the container
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services to the container.
builder.Services.AddScoped<IServiceUsers, ServiceUsers>();

// Add Service cache redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("RedisCacheURL").Get<string>();
});

var connectionDicionary = RedisConnection.GetConnectionDictionary(
    "Redis", builder.Configuration.GetSection("RedisCacheURL").Get<string>());

var connectionString = DBConnection.GetConnectionString(connectionDicionary);
Console.WriteLine("[ConnectionString] - " + connectionString);

// Add service dbcontext
//builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString,
//    sqlServerOptionsAction: sqlOptions =>
//    {
//        sqlOptions.EnableRetryOnFailure();
//    })
//);
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();