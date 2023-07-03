using System.Text;
using CarNotes.Application.Services;
using CarNotes.Domain.Interfaces.Services;
using CarNotes.FileStorage.AwsS3;
using CarNotes.FileStorage.Local;
using CarNotes.Persistence.Neo4j;
using CarNotes.WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecret ?? throw new ArgumentNullException("JWT Secret must be specified."))),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAutoMapper(typeof(ViewModelMappingProfile));

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IStatsService, StatsService>();

// File storage.
builder.Services.AddLocalFileStorage(options =>
{
    options.StoragePath =
        builder.Configuration.GetSection("LocalStorageConfiguration").GetValue<string>("StoragePath") ?? "LocalStorage/images";
});
// Not tested.
//builder.Services.AddAwsS3FileStorage(options =>
//{
//    options.AccessKey = builder.Configuration.GetSection("AwsConfiguration").GetValue<string>("AwsAccessKey");
//    options.SecretKey = builder.Configuration.GetSection("AwsConfiguration").GetValue<string>("AwsSecretKey");
//    options.BucketName = builder.Configuration.GetSection("AwsConfiguration").GetValue<string>("AwsBucketName");
//});

// Data storage.
builder.Services.AddNeo4jPersistence(options =>
{
    options.Connection = settings.Neo4jConnection;
    options.User = settings.Neo4jUser;
    options.Password = settings.Neo4jPassword;
    options.Database = settings.Neo4jDatabase;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Local file storage support.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LocalStorage")),
    RequestPath = "/static",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.UseCors("allowClientappOrigin");

app.Run();

