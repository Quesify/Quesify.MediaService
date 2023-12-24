﻿using Microsoft.OpenApi.Models;
using Quesify.SharedKernel.AspNetCore.Logging.Models;
using Quesify.SharedKernel.AspNetCore.Swagger.Filters;
using Quesify.SharedKernel.Guids;
using Quesify.SharedKernel.Storage;
using Quesify.SharedKernel.TimeProviders;
using Serilog;
using Serilog.Events;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services,
        ConfigureHostBuilder hostBuilder)
    {
        services.AddControllers();

        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo { Title = "Quesify.MediaService.Web", Version = "v1" });
            o.DocumentFilter<LowercaseDocumentFilter>();
        });

        services.AddTimeProvider(options => { options.UseUtcDateTime(services); });

        services.AddGuid(options => { options.UseSequentialGuidGenerator(services); });

        services.AddStorage(options => { options.UseLocalStorage(services); });

        services.AddEnableRequestBuffering();

        services.AddRequestTime();

        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
            .WriteTo.Seq(context.Configuration.GetValue<string>("Seq:ServerUrl")!)
            .WriteTo.Console()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
            .AddCustomEnriches(services);
        });

        services.AddPushSerilogProperties();

        services.AddCustomHttpLogging(options =>
        {
            options.LoggingLevel = LoggingLevel.ClientError;
            options.IgnoreRequestHeader = true;
            options.IgnoreRequestBody = true;
            options.IgnoreResponseHeader = true;
            options.IgnoreResponseBody = true;
        });

        return services;
    }
}