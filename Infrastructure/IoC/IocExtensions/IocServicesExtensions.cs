using Application.Interfaces;
using Application.Services;
using Infrastructure.Data.Persistence;
using Infrastructure.Data.Persistence.Repositories;
using Infrastructure.Security.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>();
            services.AddSingleton<IMyJwtSigningManager, MyJwtSigningManager>();
            services.AddScoped<IProjetoService, ProjetoService>();
            
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
