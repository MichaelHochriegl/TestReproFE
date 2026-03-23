using Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();