using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MapperProfiles>();
            });
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            return services;
        }
        }
}
