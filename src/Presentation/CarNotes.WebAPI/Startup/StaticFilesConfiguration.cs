using Microsoft.Extensions.FileProviders;

namespace CarNotes.WebAPI.Startup;

public static class StaticFilesConfiguration
{
    public static WebApplication ConfigureStaticFiles(this WebApplication app)
    {
        // Local file storage support.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "LocalStorage")),
            RequestPath = "/static",
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append(
                    "Cache-Control", "public,max-age=3600");
            }
        });

        return app;
    }
}
