using ProductsApi.Middleware;
using ProductsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Logs de la app (ILogger) también en JSON
builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole(options =>
{
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "O";
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Servicio de productos en memoria (Singleton: debe persistir los datos entre requests)
builder.Services.AddSingleton<IProductService, InMemoryProductService>();

var app = builder.Build();

// Log estructurado JSON por cada request (debe ir primero en el pipeline)
app.UseMiddleware<JsonRequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();