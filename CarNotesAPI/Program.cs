using CarNotesAPI.Data;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.Data.Models.Notes;
using CarNotesAPI.Data.Repositories;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "allowClientappOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080");
        });
});

builder.Services.Configure<ApplicationSettings>(
    builder.Configuration.GetSection("ApplicationSettings"));

var settings = new ApplicationSettings();
builder.Configuration.GetSection("ApplicationSettings").Bind(settings);

builder.Services.AddSingleton(GraphDatabase.Driver(
    settings.Neo4jConnection,
    AuthTokens.Basic(settings.Neo4jUser, settings.Neo4jPassword)));
builder.Services.AddScoped<INeo4jDataAccess, Neo4jDataAccess>();
builder.Services.AddTransient<ICarRepository, CarRepository>();
builder.Services.AddTransient<IMileageRepository, MileageRepository>();
builder.Services.AddTransient<INoteRepository<LegalProcedure>, LegalProcedureRepository>();
builder.Services.AddTransient<INoteRepository<Refueling>, RefuelingRepository>();
builder.Services.AddTransient<INoteRepository<Service>, ServiceRepository>();
builder.Services.AddTransient<INoteRepository<SparePart>, SparePartRepository>();
builder.Services.AddTransient<INoteRepository<Washing>, WashingRepository>();

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

app.UseCors("allowClientappOrigin");

app.Run();

