using System.Text;
using CarNotesAPI.Data;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using CarNotesAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neo4j.Driver;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ApplicationSettings>(
    builder.Configuration.GetSection("ApplicationSettings"));

var settings = new ApplicationSettings();
builder.Configuration.GetSection("ApplicationSettings").Bind(settings);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "allowClientappOrigin",
        policy =>
        {
            policy.WithOrigins(settings.WebClientUrl);
        });
});


builder.Services.AddSingleton(GraphDatabase.Driver(
    settings.Neo4jConnection,
    AuthTokens.Basic(settings.Neo4jUser, settings.Neo4jPassword)));
builder.Services.AddScoped<INeo4jDataAccess, Neo4jDataAccess>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICarRepository, CarRepository>();
builder.Services.AddTransient<IMileageRepository, MileageRepository>();
builder.Services.AddTransient<INoteRepository<LegalProcedure>, LegalProcedureRepository>();
builder.Services.AddTransient<INoteRepository<Refueling>, RefuelingRepository>();
builder.Services.AddTransient<IStatsRepository, StatsRepository>();
builder.Services.AddTransient<INoteRepository<Service>, ServiceRepository>();
builder.Services.AddTransient<INoteRepository<SparePart>, SparePartRepository>();
builder.Services.AddTransient<INoteRepository<Washing>, WashingRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IStatsService, StatsService>();


builder.Services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = settings.WebServerUrl,
            ValidAudience = settings.WebClientUrl,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecret ?? throw new ArgumentNullException("JWT Secret must be specified.")))
        };
    });

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

app.MapHealthChecks("/health");

app.UseCors("allowClientappOrigin");

app.Run();

