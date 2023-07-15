using System.Text;
using CarNotes.Application.Services;
using CarNotes.Domain.Common.Exceptions;
using CarNotes.Domain.Interfaces.Services;
//using CarNotes.FileStorage.AwsS3;
using CarNotes.FileStorage.Local;
using CarNotes.Persistence.Neo4j;
using CarNotes.WebAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CarNotes.WebAPI.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddConfigurationServices(configuration);

        var appSettings = configuration
            .GetRequiredSection(ApplicationSettings.SectionName)
            .Get<ApplicationSettings>() ?? throw new InvalidConfigurationException(
                $"Invalid configuration: {ApplicationSettings.SectionName}");

        // Learn more about configuring Swagger/OpenAPI at
        // https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();

        services.AddHealthChecks();

        services.AddCors(options =>
        {
            options.AddPolicy(
                name: "allowClientappOrigin",
                policy =>
                {
                    policy.WithOrigins(appSettings.WebClientUrl);
                });
        });

        services
            .AddAuthentication(options =>
            {
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
                    ValidIssuer = appSettings.WebServerUrl,
                    ValidAudience = appSettings.WebClientUrl,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtSecret ?? throw new ArgumentNullException("JWT Secret must be specified."))),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.AddAutoMapper(typeof(DtoMappingProfile));
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IImageService, ImageService>();
        services.AddTransient<IStatsService, StatsService>();

        services.AddDataStorage(configuration);
        services.AddFileStorage(configuration);

        return services;
    }

    private static IServiceCollection AddConfigurationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationSettings>(options =>
            configuration
                .GetSection(ApplicationSettings.SectionName)
                .Bind(options)
        );

        return services;
    }

    private static IServiceCollection AddDataStorage(
        this IServiceCollection services, IConfiguration configuration)
    {
        var neo4jSettings = configuration
            .GetRequiredSection(Neo4jOptions.SectionName)
            .Get<Neo4jOptions>() ?? throw new InvalidConfigurationException(
                $"Invalid configuration: {Neo4jOptions.SectionName}");

        services.AddNeo4jPersistence(options =>
        {
            options.Connection = neo4jSettings.Connection;
            options.User = neo4jSettings.User;
            options.Password = neo4jSettings.Password;
            options.Database = neo4jSettings.Database;
        });

        return services;
    }

    private static IServiceCollection AddFileStorage(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Local file storage.
        var localStorageSettings = configuration
            .GetRequiredSection(LocalStorageSettings.SectionName)
            .Get<LocalStorageSettings>() ?? throw new InvalidConfigurationException(
                $"Invalid configuration: {LocalStorageSettings.SectionName}");

        services.AddLocalFileStorage(options =>
        {
            options.StoragePath = localStorageSettings.StoragePath;
        });

        // AWS S3 file storage - Not tested.
        //var awsS3StorageSettings = configuration
        //    .GetRequiredSection(AwsS3StorageSettings.SectionName)
        //    .Get<AwsS3StorageSettings>() ?? throw new InvalidConfigurationException(
        //        $"Invalid configuration: {AwsS3StorageSettings.SectionName}");

        //services.AddAwsS3FileStorage(options =>
        //{
        //    options.AccessKey = awsS3StorageSettings.AccessKey;
        //    options.SecretKey = awsS3StorageSettings.SecretKey;
        //    options.BucketName = awsS3StorageSettings.BucketName;
        //});

        return services;
    }
}
