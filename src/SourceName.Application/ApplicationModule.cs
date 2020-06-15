using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

using AutoMapper;
using FluentValidation;
using MediatR;

using SourceName.Application.Common.Behaviors;

namespace SourceName.Application
{
    public static class ApplicationModule
    {
        public static void AddApplicationModule(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            
            services.AddAutoMapper(executingAssembly);
            services.AddMediatR(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}