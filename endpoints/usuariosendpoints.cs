namespace usuario.API.Endpoints;

using usuario.API.Models;
using usuario.API.Data;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/usuarios/login", (Login datos) =>
{
    const string adminUser = "manolo";
    const string adminPassword = "1234"; //la cedula la utilizare como password

    if (datos.Cedula == adminUser && datos.Password == adminPassword)
        return Results.Ok(new
        {
            mensaje = "Acceso concedido al Panel Administrativo",
            rol = "Admin",
            token = "fake-jwt-token-para-fase-1" // 
        });

    return Results.Json(new { mensaje = "Usuario no registrado o datos incorrectos" }, statusCode: 401);
});
        app.MapGet("/api/usuarios", (IUsuarioRepository repo) => repo.GetAll());

        app.MapPost("/api/usuarios", (Usuario nuevousuario, IUsuarioRepository repo) =>
        {
            var hoy = DateTime.Today;
            if (nuevousuario.FechaNacimiento > hoy)
                return Results.BadRequest(new { mensaje = "La fecha de nacimiento no puede ser futura." });

            if (nuevousuario.FechaNacimiento < hoy.AddYears(-120))
                return Results.BadRequest(new { mensaje = "Fecha de nacimiento demasiado antigua." });
            var usuarioConId = nuevousuario with { Id = repo.GetAll().Count + 1 };
            repo.Add(usuarioConId);
            return Results.Ok(usuarioConId);
        });

        app.MapDelete("/api/usuarios/{id}", (int id, IUsuarioRepository repo) =>
        {   var lista = repo.GetAll();
            var UsuarioEliminado = lista.FirstOrDefault(u => u.Id == id);
            if (UsuarioEliminado == null) return Results.NotFound();
            lista.Remove(UsuarioEliminado);
            return Results.Ok(new { mensaje = "Usuario eliminado", id });
        });

            app.MapPut("/api/usuarios/{id}", (int id, Usuario usuarioeditado,IUsuarioRepository repo) =>
            {
                var lista = repo.GetAll();
                var index = lista.FindIndex(u => u.Id == id);
                if (index == -1) return Results.NotFound();

                lista[index] = usuarioeditado with { Id = id };
                return Results.Ok(lista[index]);
            });


    }
}





