//using System.IO.Pipelines;
using usuario.API.Endpoints;
using usuario.API.Data;
using usuario.API.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICIOS
// herramientas de uso para la api
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IUsuarioRepository, InMemoryUsuarioRepository>();
//configuracion para que react pida cosas
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// 2. MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//obliga la conexion segura https
app.UseHttpsRedirection();
//activa las configuraciones para react y debe ir antes que las rutas
app.UseCors("NuevaPolitica");

// 3. RUTAS
app.MapUsuarioEndpoints();
//creacion de

// esta linea hace que el servidor pueda arrancar
app.Run();

