namespace CCMSApp.WebApi.Infrastructure;

public static class SwaggerConfiguration
{
    public static IApplicationBuilder UseSwaggerWithVersioning(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "CCMSApp API v1");
            options.RoutePrefix = string.Empty;
        });

        return app;
    }
}
