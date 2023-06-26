using CarNotes.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarNotes.FileStorage.AwsS3;

public static class DependencyInjection
{
    public static IServiceCollection AddAwsS3FileStorage(
        this IServiceCollection services,
        Action<S3Options> configureOptions)
    {
        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        var s3Options = new S3Options();
        configureOptions.Invoke(s3Options);
        services.AddSingleton(s3Options);

        services.AddScoped<IFileStorageService<S3FileObject>, S3Storage>();

        return services;
    }
}
