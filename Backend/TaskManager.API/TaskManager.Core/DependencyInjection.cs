using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskManager.Core.Features.Projects.ServicesContracts;
using TaskManager.Core.Features.Tasks.ServicesContracts;
using TaskManager.Core.Features.Projects.Services;
using TaskManager.Core.Features.Tasks.Services;

namespace TaskManager.Core;
public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // Register AutoMapper profiles from this assembly
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Register the services
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        return services;
    }
}

