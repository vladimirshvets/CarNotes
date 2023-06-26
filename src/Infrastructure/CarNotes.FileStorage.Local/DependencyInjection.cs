using CarNotes.Domain.Interfaces.Services;
using CarNotes.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CarNotes.FileStorage.Local
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLocalFileStorage(
            this IServiceCollection services,
            Action<StorageOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var storageOptions = new StorageOptions();
            configureOptions.Invoke(storageOptions);
            services.AddSingleton(storageOptions);

            services.AddScoped<IFileStorageService<FileObject>, LocalStorage>();

            return services;
        }
    }
}
