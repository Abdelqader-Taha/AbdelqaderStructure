using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using AbdelqaderStructure.Extensions;

namespace AbdelqaderStructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddAuthConfig();
            services.AddAutoMapperConfig();
            services.AddSignalRConfig();
            services.AddSwaggerConfig();

            services.AddServices(); // your own service registrations
            services.AddHttpContextAccessor();

            services.AddCustomRouting();
            services.AddCustomControllers();
            services.AddEndpointsApiExplorer();

            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddFluentValidationAutoValidation();
            services.AddMemoryCache();

            services.AddTmpStuff();
            services.AddControllersWithViews();
            services.AddLocalizationConfig();

            return services;
        }
    }
}
