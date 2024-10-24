using EclipseWorksChallenge.Ioc.IocExtensions;
using Infrastructure.IoC.IocExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureCoreServices(builder.Configuration);
builder.Services.ConfigureApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Importante! Atentar para a ordem de execução dos Middlewares.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
