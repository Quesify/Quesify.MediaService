var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebServices(builder.Host);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
var appStartedLogMessage = "{ApplicationName} started. Hosting environment: {Environment}";
logger.LogInformation(appStartedLogMessage);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEnableRequestBuffering();

app.UseRequestTime();

app.UsePushSerilogPropertiesMiddleware();

app.UseCustomHttpLoggingMiddleware();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.UseExceptionHandler(o => { });

app.Run();
