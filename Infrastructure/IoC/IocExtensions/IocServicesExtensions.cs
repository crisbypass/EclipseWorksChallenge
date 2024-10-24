using Application.Interfaces;
using Application.Services;
using Infrastructure.Data.Persistence;
using Infrastructure.Data.Persistence.Repositories;
using Infrastructure.Security.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.IoC.IocExtensions
{
    /// <summary>
    /// Configuração do container nativo de injeção de dependência.
    /// </summary>
    /// <remarks>
    /// Outros containeres alternativos poderiam ser uma opção, devido aos recursos adicionais 
    /// a serem oferecidos, como o AutoFac(registro automático de serviços pelo nome, interface, atributos, etc.)
    /// Mas vamos simplificar por questões significativas, pois o container nativo é mais performático.
    /// </remarks>
    public static class IocServicesExtensions
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<MyDbContext>(builder => {

                var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                    throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

                using var loggerFactory = LoggerFactory.Create(x=> x.AddConsole());

                var logger = loggerFactory.CreateLogger<MyDbContext>();

                builder.UseInMemoryDatabase(connectionString)
                    .LogTo(message => logger.Log(LogLevel.Information, message));
            });
            services.AddSingleton<IMyJwtSigningManager, MyJwtSigningManager>();
            services.AddScoped<IProjetoService, ProjetoService>();
            services.AddScoped<ITarefaService, TarefaService>();
            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnityOfWork, UnityOfWork>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IMyJwtSigningManager>((options, signingManager) =>
                {
                    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidIssuer = "Me",
                        ValidAudience = "Me",
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = signingManager.FetchCurrentEcdsaKey()
                    };
                });

            return services;
        }
    }
}
