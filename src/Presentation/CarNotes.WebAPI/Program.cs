using CarNotes.WebAPI.Startup;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.ConfigureSwagger();

app.UseHttpsRedirection();

app.ConfigureStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.UseCors("allowClientappOrigin");

app.Run();
