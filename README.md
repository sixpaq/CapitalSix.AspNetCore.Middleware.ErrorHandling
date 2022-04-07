# CapitalSix.AspNetCore.Middleware.ErrorHandling

This module captures "uncaught" exceptions and generated ProblemDetail results in json format.


### How to configure the module.
```c#
void ConfigureServices(IServicesCollection services)
{
    ...
    services
        .AddControllers()
        .AddExceptionMiddleware();
    ...
}

void Configure(IApplicationBuilder app)
{
    ...
    app.UseExceptionMiddleware();
    ...
}
```

### Customizing
It is possible to customize the middleware by adding custom mappings. For each type of
exception a mapping can be added. This mapping is responsible for the conversion
from Exception to ProblemDetails.
```c#
void ConfigureServices(IServicesCollection services)
{
    ...
    services.Configure<ExceptionMiddlewareOptions>(options =>
    {
        options.Mappings.Add<ArgumentNullException>(ex =>
             new ProblemDetails()
            {
                Status = 500,
                Title = "Arguments are not allowed to be null"
            });
    });
    ...
}
builder.Services
```