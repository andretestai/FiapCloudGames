using Core.Repository;
using Core.Services;
using Infrastructure.DataAccess.Repository;
using Infrastructure.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using Services.Services.AutoMapper;
using Services.Services.Criptografia;
using Services.Services.JWT;

namespace Services
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddServices(services);
            AddAutoMapper(services);
            AddRepositories(services);
            AddLogs(services);
            AddCriptografiaSenha(services, configuration);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(opt =>
            {
                opt.AddProfile(new AutoMapping());
            }).CreateMapper());
        }

        private static void AddCriptografiaSenha(IServiceCollection services, IConfiguration configuration)
        {
            var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

            services.AddScoped(opt => new CriptografarSenha(additionalKey!));
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserGameService, UserGameService>();
            services.AddTransient<JWTGenerator, JWTGenerator>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GamesRepository>();
            services.AddScoped<IUserGameRepository, UserGamesRepository>();
        }

        private static void AddLogs(IServiceCollection services)
        {
            services.AddTransient<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddTransient(typeof(BaseLogger<>));
        }
    }
}
