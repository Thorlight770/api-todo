using api.todo.Context;
using api.todo.Services;
using api.todo.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using api.todo.Repository;
using api.todo.Repository.Impl;
using api.todo.Utils;
using Commond_Lib.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddJsonConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var env = builder.Environment;

// Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

// Tambahkan CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? "")),
         ClockSkew = TimeSpan.Zero
     };
 });
// Jwt configuration ends here

// Add repository to the container
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services to the container.
builder.Services.AddScoped<IServiceUsers, ServiceUsers>();

string redisURL = builder.Configuration.GetSection("RedisEPV").Get<string>() ?? "";
Console.WriteLine("[RedisURL] " + redisURL);
string dbEpvID = builder.Configuration.GetSection("Database:EpvID").Get<string>() ?? "";
string dbName = builder.Configuration.GetSection("Database:Name").Get<string>() ?? "";

// Add Service cache redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisURL;
});

//var connectionDicionary = RedisConnection.GetConnectionDictionary(
//    "Redis", redisURL, dbEpvID);

var connectionDicionary = ClsEPV.GetEPV(redisURL, dbName, dbEpvID);

var connectionString = DBConnection.GetConnectionString(connectionDicionary, dbName);
Console.WriteLine("[ConnectionString] - " + connectionString);

builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Logger;
logger.LogInformation("Logger working!");



// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// Aktifkan CORS
app.UseCors("AllowLocalhost5173");


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlingGlobalMiddleware>();

app.Run();