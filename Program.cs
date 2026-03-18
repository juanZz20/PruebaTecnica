//using System.IO.Pipelines;

using Microsoft.AspNetCore.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICIOS
// herramientas de uso para la api
builder.Services.AddOpenApi();
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
var usuarios = new List<Usuario> { new Usuario(1, "1234","ramos123","raul","zapata", new DateTime(1999, 12, 10), "3502657845","calle 24c #34-33")};

//creacion de rutas

app.MapPost("/api/usuarios/login",(Login datos) =>
{
    const string adminUser= "manolo";
    const string adminPassword="manolo123"; 

    if (datos.Cedula == adminUser && datos.Password == adminPassword)
        return Results.Ok(new
        {
            mensaje = "Acceso concedido al Panel Administrativo",
            rol = "Admin",
            token = "fake-jwt-token-para-fase-1" // 
        });

    return Results.Json(new { mensaje = "Usuario no registrado o datos incorrectos" }, statusCode: 401);
});
    



app.MapGet("/api/usuarios", () => usuarios);

app.MapPost("/api/usuarios", (Usuario nuevousuario) =>
{
    var hoy = DateTime.Today;

    if (nuevousuario.FechaNacimiento > hoy)
    {
        return Results.BadRequest(new { mensaje = "La fecha de nacimiento no puede ser en el futuro." });
    }
    if (nuevousuario.FechaNacimiento < hoy.AddYears(-120))
    {
        return Results.BadRequest(new { mensaje = "La fecha de nacimiento no es válida (demasiado antigua)." });
    }
    var Usuario = nuevousuario with { Id = usuarios.Count + 1 };
    usuarios.Add(Usuario);
    return Results.Ok(Usuario);
});

app.MapDelete("/api/usuarios/{id}", (int id) =>
{
    var UsuarioEliminado = usuarios.FirstOrDefault(u => u.Id == id);
    if (UsuarioEliminado == null) return Results.NotFound();
    usuarios.Remove(UsuarioEliminado);
    return Results.Ok(new { mensaje = "Usuario eliminado", id });
});

app.MapPut("/api/usuarios/{id}", (int id, Usuario usuarioeditado) =>
{
    var index = usuarios.FindIndex(u => u.Id == id);
    if (index == -1) return Results.NotFound();

    usuarios[index] = usuarioeditado with { Id = id };
    return Results.Ok(usuarios[index]);
});


// esta linea hace que el servidor pueda arrancar
app.Run();

public record Usuario(
    int Id,
    string Cedula,
    string Password,
    string Nombre,
    string Apellidos,
    DateTime FechaNacimiento,
    string Telefono,
    string Direccion
)
{
    
    public int Edad
    {
        get
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - FechaNacimiento.Year;
            if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }
    }
}
record Login(string Cedula,string Password);