using CleanArchitecture.Application.Behaviours;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CleanArchitecture.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("CleanArchitecture.Presentation.Web.API")));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("CleanArchitecture.Application")));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("CleanArchitecture.Infrastructure.Persistence")));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        }
    }
}
