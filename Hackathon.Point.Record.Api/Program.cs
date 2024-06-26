using Hackathon.Point.Record.Api.Infra.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .ConfigContext(builder.Configuration)
    .ConfigDependencyInjection()
    .ConfigureCors()
    .ConfigBus(builder.Configuration)
    .AuthenticationConfigurations(builder.Configuration);

var app = builder.Build();

app.Services.ExecuteMigration();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("x-frame-options", "DENY");

    await next();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
