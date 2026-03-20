namespace usuario.API.Endpoints;

using usuario.API.Models;
using usuario.API.Data;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapPost("/api/usuarios/login", (LoginRequest datos) =>
        {
   
            //Console.WriteLine($"Nombre recibido: '{datos.Nombre}'");
            //Console.WriteLine($"Cedula recibida: '{datos.Cedula}'");

            const string adminUser = "manolo";
            const string adminPassword = "1234";
            //Console.WriteLine($"Comparación usuario: {datos.Nombre?.Trim().ToLower()} == {adminUser}");
            //Console.WriteLine($"Comparación clave: {datos.Cedula?.Trim()} == {adminPassword}");

            if (datos.Nombre?.Trim().ToLower() == adminUser && datos.Cedula?.Trim() == adminPassword)
            {
                return Results.Ok(new { mensaje = "Acceso concedido", rol = "Admin" });
            }

            return Results.Unauthorized();
        });
        app.MapGet("/api/usuarios", (IUsuarioRepository repo) =>
        {
            var todos = repo.GetAll();
            var filtrados = todos.Where(u => u.Id != 1).ToList();
            return Results.Ok(filtrados);
        }

        );

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

                app.MapPut("/api/usuarios/{cedula}", (string cedula, Usuario usuarioeditado,IUsuarioRepository repo) =>
                {
                    var lista = repo.GetAll();
                    var index = lista.FindIndex(c => c.Cedula == cedula);
                    if (index == -1) return Results.NotFound();

                    lista[index] = usuarioeditado with { Cedula = cedula };
                    return Results.Ok(lista[index]);
                });


    }
}





