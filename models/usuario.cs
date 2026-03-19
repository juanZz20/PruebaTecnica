
namespace usuario.API.Models;

public record Usuario(
    int Id,
    string Cedula,
    string Nombre,
    string Apellidos,
    DateTime FechaNacimiento,
    string Telefono,
    string Direccion
)
{

}
public record Login(string Cedula, string Password);