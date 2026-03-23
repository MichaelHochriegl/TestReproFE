using Data;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDatabase(connectionString);

builder.Services.AddFastEndpoints()
    .SwaggerDocument();

var app = builder.Build();

app.UseFastEndpoints(o => o.Endpoints.Configurator = (ep) => ep.AllowAnonymous());
app.UseSwaggerGen();

app.Run();